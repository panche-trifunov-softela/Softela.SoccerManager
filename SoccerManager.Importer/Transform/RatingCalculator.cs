using Microsoft.Extensions.Options;
using SoccerManager.Importer.Dataset;

namespace SoccerManager.Importer.Transform;

/// <summary>
/// Computes each imported player's overall rating (Rating v1): seven components, each in [0,1] and weighted by the
/// player's position group, combined into a composite score whose percentile across all imported players is mapped
/// through a curve to a 1-100 rating.
/// </summary>
public sealed class RatingCalculator
{
    private readonly RatingOptions _options;
    private readonly TransformOptions _transformOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="RatingCalculator"/> class.
    /// </summary>
    /// <param name="options">The rating configuration.</param>
    /// <param name="transformOptions">The transform-wide configuration, for the snapshot's reference date.</param>
    public RatingCalculator(IOptions<RatingOptions> options, IOptions<TransformOptions> transformOptions)
    {
        _options = options.Value;
        _transformOptions = transformOptions.Value;
    }

    /// <summary>
    /// Computes a rating for every player in <paramref name="players"/>.
    /// </summary>
    /// <param name="players">Each imported player's raw rating inputs.</param>
    /// <param name="clubLeagueStats">Every club's combined-window domestic league results, keyed by Transfermarkt club id.</param>
    /// <param name="clubSeasonStats">Every club's per-season domestic league results, keyed by (club id, season).</param>
    /// <param name="clubLeagueIds">The domestic competition code of every imported club, keyed by Transfermarkt club id.</param>
    /// <param name="importedClubIds">The Transfermarkt ids of every imported club, the cohort the defence and team components are ranked across.</param>
    /// <param name="windowSeasons">The two window seasons, encoded by their start year.</param>
    /// <param name="currentSeason">The current season, encoded by its start year.</param>
    /// <returns>Each player's rating, from 1 to 100, keyed by Transfermarkt player id.</returns>
    public IReadOnlyDictionary<int, int> Calculate(
        IReadOnlyList<RatingPlayerInput> players,
        IReadOnlyDictionary<int, ClubLeagueStats> clubLeagueStats,
        IReadOnlyDictionary<(int ClubId, int Season), ClubSeasonLeagueStats> clubSeasonStats,
        IReadOnlyDictionary<int, string> clubLeagueIds,
        IReadOnlySet<int> importedClubIds,
        IReadOnlyList<int> windowSeasons,
        int currentSeason)
    {
        var count = players.Count;
        var (defenceByClub, teamByClub) = RankClubs(clubLeagueStats, importedClubIds);
        var (leagueMin, leagueMax) = LeagueCoefficientRange();

        var marketValueRaw = new double[count];
        var minutesShareRaw = new double[count];
        var outputRaw = new double[count];
        var defenceScore = new double[count];
        var teamScore = new double[count];
        var leagueScore = new double[count];
        var capsRaw = new double[count];

        for (var i = 0; i < count; i++)
        {
            var player = players[i];

            marketValueRaw[i] = MarketValueRaw(player);
            minutesShareRaw[i] = MinutesShareRaw(player, clubSeasonStats, windowSeasons, currentSeason);
            outputRaw[i] = OutputRaw(player);
            capsRaw[i] = player.InternationalCaps;

            var clubId = player.ClubTransfermarktId;
            defenceScore[i] = clubId is { } club && defenceByClub.TryGetValue(club, out var defence) ? defence : 0;
            teamScore[i] = clubId is { } teamClub && teamByClub.TryGetValue(teamClub, out var team) ? team : 0;
            leagueScore[i] = LeagueScore(clubId, clubLeagueIds, leagueMin, leagueMax);
        }

        var groups = players.Select(player => player.Group).ToArray();
        var marketValuePercentile = PercentileByGroup(groups, marketValueRaw);
        var minutesPercentile = PercentileByGroup(groups, minutesShareRaw);
        var outputPercentile = PercentileByGroup(groups, outputRaw);
        var capsPercentile = Percentiles.Rank(capsRaw);

        var composite = new double[count];
        for (var i = 0; i < count; i++)
        {
            var weights = WeightsFor(players[i].Group);

            composite[i] =
                (weights.MarketValue * marketValuePercentile[i]) +
                (weights.Minutes * minutesPercentile[i]) +
                (weights.Output * outputPercentile[i]) +
                (weights.Defence * defenceScore[i]) +
                (weights.Team * teamScore[i]) +
                (weights.League * leagueScore[i]) +
                (weights.Caps * capsPercentile[i]);
        }

        var compositePercentile = Percentiles.Rank(composite);

        var ratings = new Dictionary<int, int>(count);
        for (var i = 0; i < count; i++)
        {
            var curved = ApplyCurve(compositePercentile[i]);
            var rounded = (int)Math.Round(curved, MidpointRounding.AwayFromZero);

            ratings[players[i].TransfermarktId] = Math.Clamp(rounded, 1, 100);
        }

        return ratings;
    }

    private static (IReadOnlyDictionary<int, double> Defence, IReadOnlyDictionary<int, double> Team) RankClubs(
        IReadOnlyDictionary<int, ClubLeagueStats> clubLeagueStats, IReadOnlySet<int> importedClubIds)
    {
        var cohort = importedClubIds
            .Select(clubId => clubLeagueStats.TryGetValue(clubId, out var stats) ? stats : null)
            .Where(stats => stats is { Games: > 0 })
            .Select(stats => stats!)
            .ToArray();

        var goalsAgainstPerGame = cohort.Select(stats => (double)stats.GoalsAgainst / stats.Games).ToArray();
        var pointsPerGame = cohort.Select(stats => (double)stats.Points / stats.Games).ToArray();

        var goalsAgainstPercentile = Percentiles.Rank(goalsAgainstPerGame);
        var pointsPercentile = Percentiles.Rank(pointsPerGame);

        var defence = new Dictionary<int, double>(cohort.Length);
        var team = new Dictionary<int, double>(cohort.Length);
        for (var i = 0; i < cohort.Length; i++)
        {
            defence[cohort[i].ClubTransfermarktId] = 1 - goalsAgainstPercentile[i];
            team[cohort[i].ClubTransfermarktId] = pointsPercentile[i];
        }

        return (defence, team);
    }

    private (double Min, double Max) LeagueCoefficientRange()
    {
        var values = _options.LeagueCoefficients.Values;

        // An empty configuration has no values to call Min/Max on; the sentinel (0, 0) is safe because it never
        // reaches a real division below (a club can only score if its league has a coefficient, and there are none).
        return values.Count == 0 ? (0, 0) : (values.Min(), values.Max());
    }

    private double LeagueScore(int? clubId, IReadOnlyDictionary<int, string> clubLeagueIds, double min, double max)
    {
        if (clubId is not { } club || !clubLeagueIds.TryGetValue(club, out var leagueId) ||
            !_options.LeagueCoefficients.TryGetValue(leagueId, out var coefficient))
        {
            return 0;
        }

        // A degenerate range (every configured league shares the same coefficient) has no spread to normalize
        // against; score every league that does have a coefficient at the top of the scale instead of dividing by
        // zero and yielding NaN.
        return max > min ? (coefficient - min) / (max - min) : 1.0;
    }

    private double MarketValueRaw(RatingPlayerInput player)
    {
        var age = AgeAt(player.DateOfBirth, _transformOptions.ReferenceDate);
        var adjustedValue = (double)player.Value;

        if (age < _options.YoungPremiumAge)
        {
            adjustedValue /= 1 + (_options.YoungPremiumPerYear * (_options.YoungPremiumAge - age));
        }

        return Math.Log10(1 + adjustedValue);
    }

    private double OutputRaw(RatingPlayerInput player)
    {
        var leagueMinutes = player.SeasonStats.Sum(season => season.LeagueMinutes);
        var leagueGoals = player.SeasonStats.Sum(season => season.LeagueGoals);
        var leagueAssists = player.SeasonStats.Sum(season => season.LeagueAssists);
        var totalMinutes = leagueMinutes + player.UefaMinutes;

        if (totalMinutes < _options.MinOutputMinutes)
        {
            return 0;
        }

        var involvements = leagueGoals + leagueAssists + (_options.UefaOutputWeight * (player.UefaGoals + player.UefaAssists));
        return involvements * 90.0 / totalMinutes;
    }

    private static double MinutesShareRaw(
        RatingPlayerInput player,
        IReadOnlyDictionary<(int ClubId, int Season), ClubSeasonLeagueStats> clubSeasonStats,
        IReadOnlyList<int> windowSeasons,
        int currentSeason)
    {
        var totalMinutes = 0;
        var totalAvailable = 0;

        foreach (var season in windowSeasons)
        {
            var seasonStats = player.SeasonStats.FirstOrDefault(stats => stats.Season == season);

            if (seasonStats is { LeagueAppearances: > 0, MainClubTransfermarktId: { } mainClub })
            {
                var available = clubSeasonStats.TryGetValue((mainClub, season), out var mainClubStats) ? mainClubStats.Games : 0;
                totalMinutes += seasonStats.LeagueMinutes;
                totalAvailable += available;
            }
            else if (season == currentSeason)
            {
                var available = player.ClubTransfermarktId is { } currentClub &&
                    clubSeasonStats.TryGetValue((currentClub, season), out var currentClubStats)
                    ? currentClubStats.Games
                    : 0;

                totalAvailable += available;
            }
        }

        if (totalAvailable == 0)
        {
            return 0;
        }

        return Math.Clamp(totalMinutes / (90.0 * totalAvailable), 0, 1);
    }

    private static double AgeAt(DateOnly dateOfBirth, DateOnly referenceDate)
    {
        var age = referenceDate.Year - dateOfBirth.Year;
        if (referenceDate < dateOfBirth.AddYears(age))
        {
            age--;
        }

        return age;
    }

    private RatingWeights WeightsFor(PositionGroup group) => group switch
    {
        PositionGroup.Goalkeeper => _options.Goalkeeper,
        PositionGroup.Defender => _options.Defender,
        PositionGroup.Midfield => _options.Midfield,
        PositionGroup.Attack => _options.Attack,
        _ => throw new ArgumentOutOfRangeException(nameof(group), group, "Unrecognized position group."),
    };

    private double ApplyCurve(double percentile)
    {
        var curve = _options.Curve;
        var anchors = new (double Percentile, double Rating)[]
        {
            (0.00, curve.P0),
            (0.10, curve.P10),
            (0.50, curve.P50),
            (0.90, curve.P90),
            (0.99, curve.P99),
            (1.00, curve.P100),
        };

        if (percentile <= anchors[0].Percentile)
        {
            return anchors[0].Rating;
        }

        for (var i = 1; i < anchors.Length; i++)
        {
            if (percentile > anchors[i].Percentile)
            {
                continue;
            }

            var (previousPercentile, previousRating) = anchors[i - 1];
            var (nextPercentile, nextRating) = anchors[i];
            var t = (percentile - previousPercentile) / (nextPercentile - previousPercentile);

            return previousRating + (t * (nextRating - previousRating));
        }

        return anchors[^1].Rating;
    }

    private static double[] PercentileByGroup(IReadOnlyList<PositionGroup> groups, IReadOnlyList<double> raw)
    {
        var result = new double[raw.Count];

        foreach (var group in Enum.GetValues<PositionGroup>())
        {
            var indices = new List<int>();
            for (var i = 0; i < groups.Count; i++)
            {
                if (groups[i] == group)
                {
                    indices.Add(i);
                }
            }

            if (indices.Count == 0)
            {
                continue;
            }

            var values = indices.Select(index => raw[index]).ToArray();
            var percentiles = Percentiles.Rank(values);

            for (var k = 0; k < indices.Count; k++)
            {
                result[indices[k]] = percentiles[k];
            }
        }

        return result;
    }
}
