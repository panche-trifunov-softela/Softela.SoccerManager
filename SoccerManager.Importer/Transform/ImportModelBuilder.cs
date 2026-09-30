using System.Globalization;
using Microsoft.Extensions.Options;
using SoccerManager.Domain.Enums;
using SoccerManager.Importer.Dataset;
using SoccerManager.Importer.Scope;
using SoccerManager.Importer.Transform.Model;
using SoccerManager.Importer.Wikidata;

namespace SoccerManager.Importer.Transform;

/// <summary>
/// Builds the import model from a streamed <see cref="Dataset.Dataset"/> and, optionally, its Wikidata enrichment:
/// de-duplicates stadiums, resolves each club's financial state from its kept players' values, resolves each
/// player's name, date of birth, positions and rating, and collects everything the dry-run report needs along the
/// way.
/// </summary>
public sealed class ImportModelBuilder
{
    private static readonly IReadOnlyDictionary<string, string> NationalTeamAliases = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["England"] = "United Kingdom",
        ["Scotland"] = "United Kingdom",
        ["Wales"] = "United Kingdom",
        ["Northern Ireland"] = "United Kingdom",
    };

    private static readonly DateOnly MinimumDateOfBirth = new(1900, 1, 1);

    private readonly ScopeOptions _scope;
    private readonly TransformOptions _transformOptions;
    private readonly ImagesOptions _imagesOptions;
    private readonly FinancialStateCalculator _financialStateCalculator;
    private readonly WageEstimator _wageEstimator;
    private readonly PlayerPositionBuilder _positionBuilder;
    private readonly RatingCalculator _ratingCalculator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportModelBuilder"/> class.
    /// </summary>
    /// <param name="scope">The league, UEFA competition and season scope configuration.</param>
    /// <param name="transformOptions">The transform-wide configuration.</param>
    /// <param name="imagesOptions">The placeholder image detection configuration.</param>
    /// <param name="financialStateCalculator">Classifies a club's financial standing.</param>
    /// <param name="wageEstimator">Estimates a player's weekly wage from their value.</param>
    /// <param name="positionBuilder">Builds a player's positions from their sub-position and lineup history.</param>
    /// <param name="ratingCalculator">Computes each imported player's overall rating.</param>
    public ImportModelBuilder(
        IOptions<ScopeOptions> scope,
        IOptions<TransformOptions> transformOptions,
        IOptions<ImagesOptions> imagesOptions,
        FinancialStateCalculator financialStateCalculator,
        WageEstimator wageEstimator,
        PlayerPositionBuilder positionBuilder,
        RatingCalculator ratingCalculator)
    {
        _scope = scope.Value;
        _transformOptions = transformOptions.Value;
        _imagesOptions = imagesOptions.Value;
        _financialStateCalculator = financialStateCalculator;
        _wageEstimator = wageEstimator;
        _positionBuilder = positionBuilder;
        _ratingCalculator = ratingCalculator;
    }

    /// <summary>
    /// Builds the import model.
    /// </summary>
    /// <param name="dataset">The streamed and folded dataset.</param>
    /// <param name="wikidata">The Wikidata matches by Transfermarkt id, or <see langword="null"/> when the step was skipped.</param>
    /// <returns>The built <see cref="ImportModel"/>.</returns>
    public ImportModel Build(Dataset.Dataset dataset, IReadOnlyDictionary<int, WikidataPlayer>? wikidata)
    {
        ArgumentNullException.ThrowIfNull(dataset);

        var windowSeasons = new[] { _scope.CurrentSeason - 1, _scope.CurrentSeason };
        var droppedRows = new List<DroppedRow>();

        var (stadiums, stadiumKeyByClub, stadiumMerges) = BuildStadiums(dataset.Clubs, droppedRows);
        var (teamCandidates, importedClubIds, leagueIdByClub) = BuildTeamCandidates(dataset.Clubs, stadiumKeyByClub, droppedRows);

        var nationalTeamsById = dataset.NationalTeams.ToDictionary(team => team.TransfermarktId);
        var valueByPlayer = BuildPlayerValues(dataset.Players, dataset.PlayerValuations);
        var positionStartsByPlayer = dataset.PlayerPositionStarts.ToDictionary(starts => starts.PlayerTransfermarktId);

        var playerResult = BuildPlayers(
            dataset.Players, wikidata, valueByPlayer, positionStartsByPlayer, nationalTeamsById, importedClubIds, droppedRows);

        var squadValueByClub = playerResult.KeptPlayers
            .Where(player => player.TeamTransfermarktId is not null)
            .GroupBy(player => player.TeamTransfermarktId!.Value)
            .ToDictionary(group => group.Key, group => group.Sum(player => player.Value));

        var teams = FinalizeTeams(teamCandidates, squadValueByClub);
        var (playersWithImages, placeholderUrls) = RemovePlaceholderImages(playerResult.KeptPlayers);

        var seasonStatsByPlayer = dataset.PlayerSeasonStats
            .GroupBy(stats => stats.PlayerTransfermarktId)
            .ToDictionary(group => group.Key, group => (IReadOnlyList<PlayerSeasonStats>)group.ToArray());
        var uefaStatsByPlayer = dataset.PlayerUefaStats.ToDictionary(stats => stats.PlayerTransfermarktId);

        var ratings = CalculateRatings(
            playersWithImages, seasonStatsByPlayer, uefaStatsByPlayer, dataset.ClubLeagueStats, dataset.ClubSeasonLeagueStats,
            leagueIdByClub, importedClubIds, windowSeasons);

        var players = playersWithImages
            .Select(player => new ImportPlayer(
                player.TransfermarktId,
                player.Name,
                player.DateOfBirth,
                ratings[player.TransfermarktId],
                player.Value,
                player.Wage,
                player.ImageUrl,
                player.TeamTransfermarktId,
                player.NationalTeamTransfermarktId,
                player.Group,
                player.Positions))
            .ToArray();

        var nationalTeams = BuildReferencedNationalTeams(playersWithImages, nationalTeamsById);
        var referees = BuildReferees(dataset.RefereeNames);

        var diagnostics = new ImportDiagnostics(
            dataset.TableResults,
            _scope.Leagues,
            windowSeasons,
            wikidata is null,
            dataset.Players.Count,
            wikidata?.Count ?? 0,
            playerResult.DatesOfBirthFilledFromWikidata,
            playerResult.DobMismatches,
            playerResult.CitizenshipMismatches,
            droppedRows,
            dataset.LineupPositionAnomalies,
            stadiumMerges,
            placeholderUrls,
            playerResult.PlayersWithUnimportedTeam,
            playerResult.PlayersWithUnresolvedNationalTeam);

        return new ImportModel(PositionCatalog.Positions, stadiums, teams, nationalTeams, referees, players, diagnostics);
    }

    private static (IReadOnlyList<ImportStadium> Stadiums, IReadOnlyDictionary<int, string?> StadiumKeyByClub, IReadOnlyList<StadiumMerge> Merges) BuildStadiums(
        IReadOnlyList<SourceClub> clubs, List<DroppedRow> droppedRows)
    {
        var namesByKey = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        var maxSizeByKey = new Dictionary<string, int>(StringComparer.Ordinal);
        var stadiumKeyByClub = new Dictionary<int, string?>(clubs.Count);

        foreach (var club in clubs)
        {
            var name = club.StadiumName?.Trim();
            if (string.IsNullOrEmpty(name) || club.StadiumSeats is not > 0)
            {
                stadiumKeyByClub[club.TransfermarktId] = null;
                continue;
            }

            if (name.Length > 100)
            {
                droppedRows.Add(new DroppedRow("Stadium", name, "stadium name longer than 100 characters"));
                stadiumKeyByClub[club.TransfermarktId] = null;
                continue;
            }

            var key = NameNormalizer.Normalize(name);
            stadiumKeyByClub[club.TransfermarktId] = key;

            if (!namesByKey.TryGetValue(key, out var names))
            {
                names = new List<string>();
                namesByKey[key] = names;
            }

            if (!names.Contains(name))
            {
                names.Add(name);
            }

            var seats = club.StadiumSeats!.Value;
            maxSizeByKey[key] = maxSizeByKey.TryGetValue(key, out var currentMax) ? Math.Max(currentMax, seats) : seats;
        }

        var stadiums = namesByKey
            .Select(entry => new ImportStadium(entry.Key, entry.Value[0], maxSizeByKey[entry.Key]))
            .ToArray();

        var merges = namesByKey
            .Where(entry => entry.Value.Count > 1)
            .Select(entry => new StadiumMerge(entry.Key, entry.Value))
            .ToArray();

        return (stadiums, stadiumKeyByClub, merges);
    }

    private static (IReadOnlyList<ImportTeam> Teams, IReadOnlySet<int> ImportedClubIds, IReadOnlyDictionary<int, string> LeagueIdByClub) BuildTeamCandidates(
        IReadOnlyList<SourceClub> clubs, IReadOnlyDictionary<int, string?> stadiumKeyByClub, List<DroppedRow> droppedRows)
    {
        var teams = new List<ImportTeam>();
        var leagueIdByClub = new Dictionary<int, string>(clubs.Count);

        foreach (var club in clubs)
        {
            var name = club.Name?.Trim();
            var key = club.TransfermarktId.ToString(CultureInfo.InvariantCulture);

            if (string.IsNullOrEmpty(name))
            {
                droppedRows.Add(new DroppedRow("Team", key, "blank name"));
                continue;
            }

            if (name.Length > 100)
            {
                droppedRows.Add(new DroppedRow("Team", key, "name longer than 100 characters"));
                continue;
            }

            var netTransferRecord = NetTransferRecordParser.Parse(club.NetTransferRecord);
            var stadiumKey = stadiumKeyByClub.GetValueOrDefault(club.TransfermarktId);

            teams.Add(new ImportTeam(club.TransfermarktId, name, club.LeagueId, FinancialState.VeryPoor, stadiumKey, 0m, netTransferRecord));
            leagueIdByClub[club.TransfermarktId] = club.LeagueId;
        }

        var importedClubIds = new HashSet<int>(teams.Select(team => team.TransfermarktId));
        return (teams, importedClubIds, leagueIdByClub);
    }

    private static IReadOnlyDictionary<int, decimal> BuildPlayerValues(IReadOnlyList<SourcePlayer> players, IReadOnlyList<PlayerValuation> valuations)
    {
        var latestValuation = valuations.ToDictionary(valuation => valuation.PlayerTransfermarktId, valuation => valuation.MarketValueInEur);
        var values = new Dictionary<int, decimal>(players.Count);

        foreach (var player in players)
        {
            values[player.TransfermarktId] = player.MarketValue
                ?? (latestValuation.TryGetValue(player.TransfermarktId, out var value) ? value : 0m);
        }

        return values;
    }

    private (
        IReadOnlyList<KeptPlayerCandidate> KeptPlayers,
        int DatesOfBirthFilledFromWikidata,
        IReadOnlyList<DobMismatch> DobMismatches,
        IReadOnlyList<CitizenshipMismatch> CitizenshipMismatches,
        int PlayersWithUnimportedTeam,
        int PlayersWithUnresolvedNationalTeam) BuildPlayers(
        IReadOnlyList<SourcePlayer> players,
        IReadOnlyDictionary<int, WikidataPlayer>? wikidata,
        IReadOnlyDictionary<int, decimal> valueByPlayer,
        IReadOnlyDictionary<int, PlayerPositionStarts> positionStartsByPlayer,
        IReadOnlyDictionary<int, SourceNationalTeam> nationalTeamsById,
        IReadOnlySet<int> importedClubIds,
        List<DroppedRow> droppedRows)
    {
        var kept = new List<KeptPlayerCandidate>();
        var filledFromWikidata = 0;
        var dobMismatches = new List<DobMismatch>();
        var citizenshipMismatches = new List<CitizenshipMismatch>();
        var unimportedTeam = 0;
        var unresolvedNationalTeam = 0;

        foreach (var player in players)
        {
            var key = player.TransfermarktId.ToString(CultureInfo.InvariantCulture);
            var name = player.Name?.Trim();

            if (string.IsNullOrEmpty(name))
            {
                droppedRows.Add(new DroppedRow("Player", key, "blank name"));
                continue;
            }

            if (name.Length > 100)
            {
                droppedRows.Add(new DroppedRow("Player", key, "name longer than 100 characters"));
                continue;
            }

            var wikidataMatch = wikidata is not null && wikidata.TryGetValue(player.TransfermarktId, out var match) ? match : null;
            var (dateOfBirth, dropReason, filled, mismatch) = ResolveDateOfBirth(player, name, wikidataMatch);

            if (dropReason is not null)
            {
                droppedRows.Add(new DroppedRow("Player", key, dropReason));
                continue;
            }

            if (filled)
            {
                filledFromWikidata++;
            }

            if (mismatch is not null)
            {
                dobMismatches.Add(mismatch);
            }

            var nationalTeamName = player.CurrentNationalTeamId is { } nationalTeamId && nationalTeamsById.TryGetValue(nationalTeamId, out var nationalTeam)
                ? nationalTeam.Name
                : null;

            var citizenshipMismatch = CheckCitizenshipMismatch(player, name, wikidataMatch, nationalTeamName);
            if (citizenshipMismatch is not null)
            {
                citizenshipMismatches.Add(citizenshipMismatch);
            }

            var teamId = player.CurrentClubId is { } clubId && importedClubIds.Contains(clubId) ? clubId : (int?)null;
            if (player.CurrentClubId is not null && teamId is null)
            {
                unimportedTeam++;
            }

            var nationalTeamRefId = player.CurrentNationalTeamId is { } wantedNationalTeamId && nationalTeamsById.ContainsKey(wantedNationalTeamId)
                ? wantedNationalTeamId
                : (int?)null;
            if (player.CurrentNationalTeamId is not null && nationalTeamRefId is null)
            {
                unresolvedNationalTeam++;
            }

            var positionStarts = positionStartsByPlayer.GetValueOrDefault(player.TransfermarktId);
            var positions = _positionBuilder.Build(player.SubPosition, positionStarts);
            var group = ResolveGroup(player.Position, positions);

            var value = valueByPlayer.GetValueOrDefault(player.TransfermarktId, 0m);
            var wage = _wageEstimator.Estimate(value);
            var imageUrl = player.ImageUrl?.Trim() is { Length: > 0 and <= 500 } url ? url : null;

            kept.Add(new KeptPlayerCandidate(
                player.TransfermarktId, name, dateOfBirth!.Value, value, wage, imageUrl, teamId, nationalTeamRefId,
                player.InternationalCaps, group, positions));
        }

        return (kept, filledFromWikidata, dobMismatches, citizenshipMismatches, unimportedTeam, unresolvedNationalTeam);
    }

    private (DateOnly? DateOfBirth, string? DropReason, bool FilledFromWikidata, DobMismatch? Mismatch) ResolveDateOfBirth(
        SourcePlayer player, string name, WikidataPlayer? wikidata)
    {
        DateOnly? transfermarktDate = null;
        if (player.DateOfBirthText is { Length: >= 10 } text &&
            DateOnly.TryParseExact(text[..10], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
        {
            transfermarktDate = parsed;
        }

        var wikidataDayDates = wikidata?.DatesOfBirth.Where(date => date.Precision == 11).ToArray() ?? Array.Empty<WikidataDateOfBirth>();

        if (transfermarktDate is { } tmDate)
        {
            var tmText = tmDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var mismatch = wikidataDayDates.Length > 0 && !wikidataDayDates.Any(date => string.Equals(date.Date, tmText, StringComparison.Ordinal))
                ? new DobMismatch(player.TransfermarktId, name, tmText, wikidataDayDates.Select(date => date.Date).ToArray())
                : null;

            return ValidateDateOfBirth(tmDate, false, mismatch);
        }

        if (wikidataDayDates.Length == 1 &&
            DateOnly.TryParseExact(wikidataDayDates[0].Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromWikidata))
        {
            return ValidateDateOfBirth(fromWikidata, true, null);
        }

        return (null, "no date of birth", false, null);
    }

    private (DateOnly? DateOfBirth, string? DropReason, bool FilledFromWikidata, DobMismatch? Mismatch) ValidateDateOfBirth(
        DateOnly date, bool filled, DobMismatch? mismatch)
    {
        if (date <= MinimumDateOfBirth || date >= _transformOptions.ReferenceDate)
        {
            return (null, "date of birth out of range", false, null);
        }

        return (date, null, filled, mismatch);
    }

    private static CitizenshipMismatch? CheckCitizenshipMismatch(
        SourcePlayer player, string name, WikidataPlayer? wikidata, string? nationalTeamName)
    {
        if (nationalTeamName is null || wikidata is null || wikidata.Citizenships.Count == 0)
        {
            return null;
        }

        var expected = NationalTeamAliases.GetValueOrDefault(nationalTeamName, nationalTeamName);
        var matches = wikidata.Citizenships.Any(citizenship => string.Equals(citizenship.Label, expected, StringComparison.Ordinal));

        if (matches)
        {
            return null;
        }

        return new CitizenshipMismatch(player.TransfermarktId, name, nationalTeamName, wikidata.Citizenships.Select(c => c.Label).ToArray());
    }

    private static PositionGroup ResolveGroup(string? rawPosition, IReadOnlyList<ImportPlayerPosition> positions)
    {
        switch (rawPosition)
        {
            case "Goalkeeper":
                return PositionGroup.Goalkeeper;
            case "Defender":
                return PositionGroup.Defender;
            case "Midfield":
                return PositionGroup.Midfield;
            case "Attack":
                return PositionGroup.Attack;
        }

        if (positions.Count > 0 && PositionCatalog.AreaOf(positions[0].PositionName) is { } area)
        {
            return area switch
            {
                PositionArea.Goalkeeper => PositionGroup.Goalkeeper,
                PositionArea.Defence => PositionGroup.Defender,
                PositionArea.Midfield => PositionGroup.Midfield,
                PositionArea.Attack => PositionGroup.Attack,
                _ => PositionGroup.Midfield,
            };
        }

        return PositionGroup.Midfield;
    }

    private IReadOnlyList<ImportTeam> FinalizeTeams(IReadOnlyList<ImportTeam> candidates, IReadOnlyDictionary<int, decimal> squadValueByClub)
    {
        return candidates
            .Select(candidate =>
            {
                var squadValue = squadValueByClub.GetValueOrDefault(candidate.TransfermarktId, 0m);
                var financialState = _financialStateCalculator.Calculate(squadValue, candidate.NetTransferRecord);

                return candidate with { SquadValue = squadValue, FinancialState = financialState };
            })
            .ToArray();
    }

    private (IReadOnlyList<KeptPlayerCandidate> Players, IReadOnlyList<string> RemovedUrls) RemovePlaceholderImages(
        IReadOnlyList<KeptPlayerCandidate> players)
    {
        var counts = players
            .Where(player => player.ImageUrl is not null)
            .GroupBy(player => player.ImageUrl!)
            .ToDictionary(group => group.Key, group => group.Count());

        var placeholders = counts
            .Where(entry => entry.Value >= _imagesOptions.PlaceholderMinShare)
            .Select(entry => entry.Key)
            .ToArray();

        if (placeholders.Length == 0)
        {
            return (players, Array.Empty<string>());
        }

        var placeholderSet = new HashSet<string>(placeholders, StringComparer.Ordinal);
        var result = players
            .Select(player => player.ImageUrl is not null && placeholderSet.Contains(player.ImageUrl) ? player with { ImageUrl = null } : player)
            .ToArray();

        return (result, placeholders);
    }

    private IReadOnlyDictionary<int, int> CalculateRatings(
        IReadOnlyList<KeptPlayerCandidate> players,
        IReadOnlyDictionary<int, IReadOnlyList<PlayerSeasonStats>> seasonStatsByPlayer,
        IReadOnlyDictionary<int, PlayerUefaStats> uefaStatsByPlayer,
        IReadOnlyList<ClubLeagueStats> clubLeagueStats,
        IReadOnlyList<ClubSeasonLeagueStats> clubSeasonStats,
        IReadOnlyDictionary<int, string> leagueIdByClub,
        IReadOnlySet<int> importedClubIds,
        IReadOnlyList<int> windowSeasons)
    {
        var ratingInputs = players
            .Select(player =>
            {
                var seasons = seasonStatsByPlayer.GetValueOrDefault(player.TransfermarktId, Array.Empty<PlayerSeasonStats>());
                var uefa = uefaStatsByPlayer.GetValueOrDefault(player.TransfermarktId);

                return new RatingPlayerInput(
                    player.TransfermarktId, player.Group, player.Value, player.DateOfBirth, player.TeamTransfermarktId,
                    player.InternationalCaps, seasons, uefa?.Minutes ?? 0, uefa?.Goals ?? 0, uefa?.Assists ?? 0);
            })
            .ToArray();

        var clubLeagueStatsById = clubLeagueStats.ToDictionary(stats => stats.ClubTransfermarktId);
        var clubSeasonStatsByKey = clubSeasonStats.ToDictionary(stats => (stats.ClubTransfermarktId, stats.Season));

        return _ratingCalculator.Calculate(
            ratingInputs, clubLeagueStatsById, clubSeasonStatsByKey, leagueIdByClub, importedClubIds, windowSeasons, _scope.CurrentSeason);
    }

    private static IReadOnlyList<ImportNationalTeam> BuildReferencedNationalTeams(
        IReadOnlyList<KeptPlayerCandidate> players, IReadOnlyDictionary<int, SourceNationalTeam> nationalTeamsById)
    {
        var referencedIds = players
            .Where(player => player.NationalTeamTransfermarktId is not null)
            .Select(player => player.NationalTeamTransfermarktId!.Value)
            .Distinct()
            .OrderBy(id => id);

        var result = new List<ImportNationalTeam>();
        foreach (var id in referencedIds)
        {
            if (!nationalTeamsById.TryGetValue(id, out var team))
            {
                continue;
            }

            var logoUrl = team.ImageUrl?.Trim() is { Length: > 0 and <= 500 } url ? url : null;
            result.Add(new ImportNationalTeam(team.TransfermarktId, team.Name, logoUrl));
        }

        return result;
    }

    private static IReadOnlyList<ImportReferee> BuildReferees(IReadOnlyList<string> refereeNames)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        var result = new List<ImportReferee>();

        foreach (var raw in refereeNames)
        {
            var name = CollapseWhitespace(raw.Trim());
            if (string.IsNullOrEmpty(name) || name.Length > 100)
            {
                continue;
            }

            if (seen.Add(name))
            {
                result.Add(new ImportReferee(name));
            }
        }

        return result;
    }

    private static string CollapseWhitespace(string value)
    {
        var parts = value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(' ', parts);
    }
}
