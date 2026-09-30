using System.Globalization;
using SoccerManager.Importer.Transform;
using SoccerManager.Importer.Transform.Model;

namespace SoccerManager.Importer.Report;

/// <summary>
/// Writes the dry-run review report to a <see cref="TextWriter"/> as plain, fixed-width text: the nine sections a
/// developer reviews before a real import runs, in order: provenance, scope, Wikidata, the import model's own
/// counts, ratings, clubs, wages, positions, and referees/stadiums/images. Nothing is written to disk.
/// </summary>
public sealed class DryRunReport
{
    /// <summary>
    /// Writes every section of the report to <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">The writer the report is printed to. The caller passes <see cref="Console.Out"/> for a dry run.</param>
    /// <param name="model">The import model to report on.</param>
    public void Write(TextWriter writer, ImportModel model)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(model);

        writer.WriteLine("SoccerManager.Importer - dry-run report");
        writer.WriteLine();

        WriteProvenance(writer, model.Diagnostics);
        WriteScope(writer, model);
        WriteWikidata(writer, model.Diagnostics);
        WriteImportModelSummary(writer, model);
        WriteRatings(writer, model);
        WriteClubsByLeague(writer, model);
        WriteWages(writer, model);
        WritePositions(writer, model);
        WriteReferees(writer, model);
    }

    private static void WriteProvenance(TextWriter writer, ImportDiagnostics diagnostics)
    {
        writer.WriteLine("== 1. Provenance ==");
        writer.WriteLine(ReportTable.Row(
            ReportTable.Left("Table", 16),
            ReportTable.Right("Bytes", 10),
            ReportTable.Right("Rows read", 10),
            ReportTable.Right("Rows kept", 10),
            ReportTable.Left("SHA-256 (16)", 16),
            ReportTable.Right("Duration", 8)));

        foreach (var result in diagnostics.TableResults)
        {
            var hashPrefix = result.ReadResult.Sha256[..Math.Min(16, result.ReadResult.Sha256.Length)];

            writer.WriteLine(ReportTable.Row(
                ReportTable.Left(result.ReadResult.Table, 16),
                ReportTable.Right(ReportNumberFormat.Megabytes(result.ReadResult.Bytes), 10),
                ReportTable.Right(ReportNumberFormat.Count(result.ReadResult.RowsRead), 10),
                ReportTable.Right(ReportNumberFormat.Count(result.RowsKept), 10),
                ReportTable.Left(hashPrefix, 16),
                ReportTable.Right(ReportNumberFormat.Seconds(result.ReadResult.Duration), 8)));
        }

        writer.WriteLine();
    }

    private static void WriteScope(TextWriter writer, ImportModel model)
    {
        var diagnostics = model.Diagnostics;

        writer.WriteLine("== 2. Scope ==");
        writer.WriteLine($"Leagues: {string.Join(", ", diagnostics.Leagues)}");
        writer.WriteLine($"Window seasons: {string.Join(", ", diagnostics.WindowSeasons.Select(FormatSeason))}");
        writer.WriteLine();

        var leagueByClub = model.Teams.ToDictionary(team => team.TransfermarktId, team => team.LeagueId);
        var clubCountByLeague = model.Teams.GroupBy(team => team.LeagueId, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.Count(), StringComparer.Ordinal);
        var playerCountByLeague = model.Players
            .Where(player => player.TeamTransfermarktId is { } clubId && leagueByClub.ContainsKey(clubId))
            .GroupBy(player => leagueByClub[player.TeamTransfermarktId!.Value], StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.Count(), StringComparer.Ordinal);

        writer.WriteLine(ReportTable.Row(ReportTable.Left("League", 8), ReportTable.Right("Clubs", 8), ReportTable.Right("Players", 8)));
        foreach (var league in diagnostics.Leagues)
        {
            writer.WriteLine(ReportTable.Row(
                ReportTable.Left(league, 8),
                ReportTable.Right(ReportNumberFormat.Count(clubCountByLeague.GetValueOrDefault(league)), 8),
                ReportTable.Right(ReportNumberFormat.Count(playerCountByLeague.GetValueOrDefault(league)), 8)));
        }

        writer.WriteLine();
    }

    private static void WriteWikidata(TextWriter writer, ImportDiagnostics diagnostics)
    {
        writer.WriteLine("== 3. Wikidata ==");

        if (diagnostics.WikidataSkipped)
        {
            writer.WriteLine("Skipped.");
            writer.WriteLine();
            return;
        }

        writer.WriteLine($"Requested: {ReportNumberFormat.Count(diagnostics.WikidataRequested)}");
        writer.WriteLine($"Matched: {ReportNumberFormat.Count(diagnostics.WikidataMatched)}");
        writer.WriteLine($"Dates of birth filled: {ReportNumberFormat.Count(diagnostics.DatesOfBirthFilledFromWikidata)}");
        writer.WriteLine();

        writer.WriteLine($"DOB mismatches: {ReportNumberFormat.Count(diagnostics.DobMismatches.Count)} (first 20)");
        foreach (var mismatch in diagnostics.DobMismatches.Take(20))
        {
            writer.WriteLine($"  {mismatch.TransfermarktId,-10} {mismatch.Name,-28} {mismatch.TransfermarktDate} vs {string.Join(", ", mismatch.WikidataDates)}");
        }

        writer.WriteLine();

        writer.WriteLine($"Citizenship mismatches: {ReportNumberFormat.Count(diagnostics.CitizenshipMismatches.Count)} (first 10)");
        foreach (var mismatch in diagnostics.CitizenshipMismatches.Take(10))
        {
            writer.WriteLine($"  {mismatch.TransfermarktId,-10} {mismatch.Name,-28} {mismatch.NationalTeamName} vs {string.Join(", ", mismatch.WikidataCitizenships)}");
        }

        writer.WriteLine();
    }

    private static void WriteImportModelSummary(TextWriter writer, ImportModel model)
    {
        var diagnostics = model.Diagnostics;

        writer.WriteLine("== 4. Import model ==");
        writer.WriteLine($"Positions: {model.Positions.Count}");
        writer.WriteLine($"Stadiums: {model.Stadiums.Count}");
        writer.WriteLine($"Teams: {model.Teams.Count}");
        writer.WriteLine($"National teams: {model.NationalTeams.Count}");
        writer.WriteLine($"Referees: {model.Referees.Count}");
        writer.WriteLine($"Players: {model.Players.Count}");
        writer.WriteLine($"Players with unimported team: {ReportNumberFormat.Count(diagnostics.PlayersWithUnimportedTeam)}");
        writer.WriteLine($"Players with unresolved national team: {ReportNumberFormat.Count(diagnostics.PlayersWithUnresolvedNationalTeam)}");
        writer.WriteLine();

        writer.WriteLine($"Dropped rows: {ReportNumberFormat.Count(diagnostics.DroppedRows.Count)}");
        foreach (var entityGroup in diagnostics.DroppedRows.GroupBy(row => row.Entity, StringComparer.Ordinal))
        {
            writer.WriteLine($"  {entityGroup.Key}:");

            var reasonsByCountDescending = entityGroup
                .GroupBy(row => row.Reason, StringComparer.Ordinal)
                .OrderByDescending(reasonGroup => reasonGroup.Count())
                .Take(10);

            foreach (var reasonGroup in reasonsByCountDescending)
            {
                writer.WriteLine($"    {reasonGroup.Key}: {ReportNumberFormat.Count(reasonGroup.Count())}");
            }
        }

        writer.WriteLine();
    }

    private static void WriteRatings(TextWriter writer, ImportModel model)
    {
        writer.WriteLine("== 5. Ratings ==");

        if (model.Players.Count == 0)
        {
            writer.WriteLine("No players.");
            writer.WriteLine();
            return;
        }

        var ratings = model.Players.Select(player => player.Rating).ToArray();

        writer.WriteLine("Histogram (5-point bands):");
        foreach (var (band, count) in ReportStatistics.HistogramBands(ratings))
        {
            writer.WriteLine($"  {band,-7} {ReportNumberFormat.Count(count)}");
        }

        writer.WriteLine();

        var sortedRatings = ratings.Select(rating => (double)rating).OrderBy(rating => rating).ToArray();
        var p10 = ReportNumberFormat.OneDecimal(ReportStatistics.PercentileOf(sortedRatings, 0.10));
        var p50 = ReportNumberFormat.OneDecimal(ReportStatistics.PercentileOf(sortedRatings, 0.50));
        var p90 = ReportNumberFormat.OneDecimal(ReportStatistics.PercentileOf(sortedRatings, 0.90));
        writer.WriteLine($"p10: {p10}  p50: {p50}  p90: {p90}");
        writer.WriteLine();

        var teamsById = model.Teams.ToDictionary(team => team.TransfermarktId);

        writer.WriteLine("Top 30:");
        writer.WriteLine(ReportTable.Row(
            ReportTable.Right("Rating", 6),
            ReportTable.Left("Name", 28),
            ReportTable.Left("Club", 24),
            ReportTable.Left("Group", 10),
            ReportTable.Right("Value", 10)));

        var top30 = model.Players.OrderByDescending(player => player.Rating).ThenByDescending(player => player.Value).Take(30);
        foreach (var player in top30)
        {
            var club = player.TeamTransfermarktId is { } clubId && teamsById.TryGetValue(clubId, out var team) ? team.Name : "-";

            writer.WriteLine(ReportTable.Row(
                ReportTable.Right(player.Rating.ToString(CultureInfo.InvariantCulture), 6),
                ReportTable.Left(player.Name, 28),
                ReportTable.Left(club, 24),
                ReportTable.Left(player.Group.ToString(), 10),
                ReportTable.Right(ReportNumberFormat.EurMillions(player.Value), 10)));
        }

        writer.WriteLine();

        writer.WriteLine("Average rating by position group:");
        foreach (var group in model.Players.GroupBy(player => player.Group).OrderBy(group => group.Key))
        {
            writer.WriteLine($"  {group.Key,-12} {ReportNumberFormat.OneDecimal(group.Average(player => player.Rating))}");
        }

        writer.WriteLine();

        writer.WriteLine("Average rating by league:");
        foreach (var league in model.Diagnostics.Leagues)
        {
            var leaguePlayers = model.Players
                .Where(player => player.TeamTransfermarktId is { } clubId
                    && teamsById.TryGetValue(clubId, out var team)
                    && string.Equals(team.LeagueId, league, StringComparison.Ordinal))
                .ToArray();

            if (leaguePlayers.Length == 0)
            {
                continue;
            }

            writer.WriteLine($"  {league,-8} {ReportNumberFormat.OneDecimal(leaguePlayers.Average(player => player.Rating))}");
        }

        writer.WriteLine();
    }

    private static void WriteClubsByLeague(TextWriter writer, ImportModel model)
    {
        writer.WriteLine("== 6. Clubs by league ==");

        foreach (var league in model.Diagnostics.Leagues)
        {
            var clubs = model.Teams
                .Where(team => string.Equals(team.LeagueId, league, StringComparison.Ordinal))
                .OrderByDescending(team => team.SquadValue)
                .ToArray();

            if (clubs.Length == 0)
            {
                continue;
            }

            writer.WriteLine($"{league}:");
            writer.WriteLine(ReportTable.Row(
                ReportTable.Left("Name", 28),
                ReportTable.Right("Squad value", 12),
                ReportTable.Right("Net transfer", 12),
                ReportTable.Left("Financial state", 14)));

            foreach (var club in clubs)
            {
                var netTransfer = club.NetTransferRecord is { } record ? ReportNumberFormat.EurMillions(record) : "-";

                writer.WriteLine(ReportTable.Row(
                    ReportTable.Left(club.Name, 28),
                    ReportTable.Right(ReportNumberFormat.EurMillions(club.SquadValue), 12),
                    ReportTable.Right(netTransfer, 12),
                    ReportTable.Left(club.FinancialState.ToString(), 14)));
            }

            writer.WriteLine();
        }
    }

    private static void WriteWages(TextWriter writer, ImportModel model)
    {
        writer.WriteLine("== 7. Wages ==");

        if (model.Players.Count == 0)
        {
            writer.WriteLine("No players.");
            writer.WriteLine();
            return;
        }

        var sortedWages = model.Players.Select(player => (double)player.Wage).OrderBy(wage => wage).ToArray();
        var min = ReportNumberFormat.Eur((decimal)sortedWages[0]);
        var median = ReportNumberFormat.Eur((decimal)ReportStatistics.PercentileOf(sortedWages, 0.50));
        var p90 = ReportNumberFormat.Eur((decimal)ReportStatistics.PercentileOf(sortedWages, 0.90));
        var max = ReportNumberFormat.Eur((decimal)sortedWages[^1]);
        writer.WriteLine($"Weekly min: {min}  median: {median}  p90: {p90}  max: {max}");
        writer.WriteLine();

        writer.WriteLine("Top 10:");
        foreach (var player in model.Players.OrderByDescending(player => player.Wage).Take(10))
        {
            writer.WriteLine($"  {player.Name,-28} {ReportNumberFormat.Eur(player.Wage)}");
        }

        writer.WriteLine();
    }

    private static void WritePositions(TextWriter writer, ImportModel model)
    {
        writer.WriteLine("== 8. Positions ==");

        var playersWithMainPosition = model.Players.Count(player => player.Positions.Count > 0);
        var secondaryPositionsTotal = model.Players.Sum(player => Math.Max(0, player.Positions.Count - 1));

        writer.WriteLine($"Players with a main position: {ReportNumberFormat.Count(playersWithMainPosition)}");
        writer.WriteLine($"Secondary positions total: {ReportNumberFormat.Count(secondaryPositionsTotal)}");
        writer.WriteLine();

        writer.WriteLine("Lineup position anomalies:");
        foreach (var anomaly in model.Diagnostics.LineupPositionAnomalies.OrderByDescending(entry => entry.Value))
        {
            var rawValue = string.IsNullOrEmpty(anomaly.Key) ? "(blank)" : anomaly.Key;
            writer.WriteLine($"  {rawValue,-20} {ReportNumberFormat.Count(anomaly.Value)}");
        }

        writer.WriteLine();
    }

    private static void WriteReferees(TextWriter writer, ImportModel model)
    {
        writer.WriteLine("== 9. Referees, stadiums, images ==");

        writer.WriteLine($"Referees: {ReportNumberFormat.Count(model.Referees.Count)}");

        var nearDuplicateRefereeGroups = model.Referees
            .GroupBy(referee => NameNormalizer.Normalize(referee.Name), StringComparer.Ordinal)
            .Where(group => group.Count() > 1)
            .ToArray();

        writer.WriteLine($"Near-duplicate referee groups: {ReportNumberFormat.Count(nearDuplicateRefereeGroups.Length)}");
        foreach (var group in nearDuplicateRefereeGroups)
        {
            writer.WriteLine($"  {string.Join(" / ", group.Select(referee => referee.Name))}");
        }

        writer.WriteLine();

        writer.WriteLine($"Stadiums: {ReportNumberFormat.Count(model.Stadiums.Count)}");
        writer.WriteLine($"Merged stadium groups: {ReportNumberFormat.Count(model.Diagnostics.StadiumMerges.Count)}");
        foreach (var merge in model.Diagnostics.StadiumMerges)
        {
            writer.WriteLine($"  {string.Join(" / ", merge.Names)}");
        }

        writer.WriteLine();

        writer.WriteLine($"Placeholder image URLs removed: {ReportNumberFormat.Count(model.Diagnostics.PlaceholderImageUrlsRemoved.Count)}");
        foreach (var url in model.Diagnostics.PlaceholderImageUrlsRemoved)
        {
            writer.WriteLine($"  {url}");
        }
    }

    // Encodes a window season by its start year into "YYYY/YY", matching how the dataset itself encodes a season.
    private static string FormatSeason(int startYear) => $"{startYear}/{(startYear + 1) % 100:D2}";
}
