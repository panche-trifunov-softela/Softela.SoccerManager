using CsvHelper;

namespace SoccerManager.Importer.Dataset;

/// <summary>
/// Accumulates the window games as the games table streams: a game is kept when its season falls in the window and
/// its competition is one of the configured leagues or UEFA competitions. For league games with both goals
/// present, this also aggregates each club's league results, both per season and combined across the window, and
/// collects the raw referee names recorded on those games.
/// </summary>
internal sealed class GamesAccumulator
{
    private readonly HashSet<string> _leagues;
    private readonly HashSet<string> _uefaCompetitions;
    private readonly HashSet<int> _windowSeasons;
    private readonly Dictionary<int, WindowGame> _windowGames = new();
    private readonly Dictionary<(int Club, int Season), ClubTotals> _clubSeasonTotals = new();
    private readonly Dictionary<int, ClubTotals> _clubTotals = new();
    private readonly List<string> _refereeNames = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="GamesAccumulator"/> class.
    /// </summary>
    /// <param name="leagues">The Transfermarkt domestic competition codes in scope.</param>
    /// <param name="uefaCompetitions">The Transfermarkt UEFA competition codes in scope.</param>
    /// <param name="windowSeasons">The window seasons, encoded by their start years.</param>
    public GamesAccumulator(HashSet<string> leagues, HashSet<string> uefaCompetitions, HashSet<int> windowSeasons)
    {
        _leagues = leagues;
        _uefaCompetitions = uefaCompetitions;
        _windowSeasons = windowSeasons;
    }

    /// <summary>The window games kept so far, by game id.</summary>
    public IReadOnlyDictionary<int, WindowGame> WindowGames => _windowGames;

    /// <summary>Each club's league results, aggregated per window season.</summary>
    public IReadOnlyList<ClubSeasonLeagueStats> ClubSeasonLeagueStats => _clubSeasonTotals
        .Select(pair => new ClubSeasonLeagueStats(pair.Key.Club, pair.Key.Season, pair.Value.Games, pair.Value.Points, pair.Value.GoalsFor, pair.Value.GoalsAgainst))
        .ToArray();

    /// <summary>Each club's league results, aggregated across the whole window.</summary>
    public IReadOnlyList<ClubLeagueStats> ClubLeagueStats => _clubTotals
        .Select(pair => new ClubLeagueStats(pair.Key, pair.Value.Games, pair.Value.Points, pair.Value.GoalsFor, pair.Value.GoalsAgainst))
        .ToArray();

    /// <summary>The raw, non-blank referee names collected from league window games.</summary>
    public IReadOnlyList<string> RefereeNames => _refereeNames;

    /// <summary>Folds one games row into this accumulator, keeping it only when it is a window game.</summary>
    /// <param name="row">The current CSV record.</param>
    public void AddRow(IReaderRow row)
    {
        var competitionId = row.GetField("competition_id");
        var season = row.GetField<int?>("season");

        if (string.IsNullOrEmpty(competitionId) || season is null || !_windowSeasons.Contains(season.Value))
        {
            return;
        }

        var isLeague = _leagues.Contains(competitionId);

        if (!isLeague && !_uefaCompetitions.Contains(competitionId))
        {
            return;
        }

        var gameId = row.GetField<int>("game_id");
        _windowGames[gameId] = new WindowGame(competitionId, season.Value, isLeague);

        if (!isLeague)
        {
            return;
        }

        var refereeName = CsvFieldReader.NullIfBlank(row.GetField("referee"));
        if (refereeName is not null)
        {
            _refereeNames.Add(refereeName);
        }

        var homeGoals = row.GetField<int?>("home_club_goals");
        var awayGoals = row.GetField<int?>("away_club_goals");

        if (homeGoals is null || awayGoals is null)
        {
            return;
        }

        var homeClubId = row.GetField<int>("home_club_id");
        var awayClubId = row.GetField<int>("away_club_id");

        AddClubResult(homeClubId, season.Value, homeGoals.Value, awayGoals.Value);
        AddClubResult(awayClubId, season.Value, awayGoals.Value, homeGoals.Value);
    }

    private void AddClubResult(int clubId, int season, int goalsFor, int goalsAgainst)
    {
        var points = goalsFor > goalsAgainst ? 3 : goalsFor == goalsAgainst ? 1 : 0;

        GetOrAdd(_clubSeasonTotals, (clubId, season)).Add(points, goalsFor, goalsAgainst);
        GetOrAdd(_clubTotals, clubId).Add(points, goalsFor, goalsAgainst);
    }

    private static ClubTotals GetOrAdd<TKey>(Dictionary<TKey, ClubTotals> totals, TKey key)
        where TKey : notnull
    {
        if (!totals.TryGetValue(key, out var current))
        {
            current = new ClubTotals();
            totals[key] = current;
        }

        return current;
    }
}
