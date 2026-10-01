using SoccerManager.Application.Commands.Player.CreatePlayer;
using SoccerManager.Application.Commands.Player.UpdatePlayer;
using SoccerManager.Application.Dtos;
using SoccerManager.Importer.Transform.Model;

namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// Plans the player writes: matched by Transfermarkt id, with the team and national team links compared by their
/// own Transfermarkt id rather than by database id, so a duplicate or hand-made row never fakes a change.
/// </summary>
public static class PlayerChangePlanner
{
    /// <summary>
    /// Builds the players plan, seeding <paramref name="ids"/> with the database id matched for every player Transfermarkt id.
    /// </summary>
    /// <param name="importPlayers">The import model's players.</param>
    /// <param name="existingPlayers">The database's existing players.</param>
    /// <param name="importTeams">The import model's teams, used to display a team link by name.</param>
    /// <param name="existingTeams">The database's existing teams, used to map an existing player's TeamId back to a Transfermarkt id and a name.</param>
    /// <param name="importNationalTeams">The import model's national teams, used to display a national team link by name.</param>
    /// <param name="existingNationalTeams">The database's existing national teams, used to map an existing player's NationalTeamId back to a Transfermarkt id and a name.</param>
    /// <param name="ids">The id lookups to read team and national team ids from and to seed <see cref="ImportIds.Players"/> on.</param>
    /// <returns>The players entity plan.</returns>
    public static EntityPlan Plan(
        IReadOnlyList<ImportPlayer> importPlayers,
        IReadOnlyList<PlayerDto> existingPlayers,
        IReadOnlyList<ImportTeam> importTeams,
        IReadOnlyList<TeamDto> existingTeams,
        IReadOnlyList<ImportNationalTeam> importNationalTeams,
        IReadOnlyList<NationalTeamDto> existingNationalTeams,
        ImportIds ids)
    {
        var writes = new List<RowWrite>();
        var unchanged = 0;

        var existingByTransfermarktId = existingPlayers
            .Where(player => player.TransfermarktId.HasValue)
            .ToDictionary(player => player.TransfermarktId!.Value, player => player);

        var teamTransfermarktIdByExistingId = existingTeams.Where(team => team.TransfermarktId.HasValue).ToDictionary(team => team.Id, team => team.TransfermarktId!.Value);
        var nationalTeamTransfermarktIdByExistingId = existingNationalTeams.Where(team => team.TransfermarktId.HasValue).ToDictionary(team => team.Id, team => team.TransfermarktId!.Value);
        var teamNameByExistingId = existingTeams.ToDictionary(team => team.Id, team => team.Name);
        var nationalTeamNameByExistingId = existingNationalTeams.ToDictionary(team => team.Id, team => team.Name);
        var teamNameByTransfermarktId = importTeams.ToDictionary(team => team.TransfermarktId, team => team.Name);
        var nationalTeamNameByTransfermarktId = importNationalTeams.ToDictionary(team => team.TransfermarktId, team => team.Name);

        foreach (var importPlayer in importPlayers)
        {
            if (existingByTransfermarktId.TryGetValue(importPlayer.TransfermarktId, out var existing))
            {
                ids.Players.Set(importPlayer.TransfermarktId, existing.Id);

                var changes = new List<FieldChange>();

                if (!string.Equals(existing.Name, importPlayer.Name, StringComparison.Ordinal))
                {
                    changes.Add(new FieldChange("Name", existing.Name, importPlayer.Name));
                }

                if (existing.DateOfBirth != importPlayer.DateOfBirth)
                {
                    changes.Add(new FieldChange("DateOfBirth", existing.DateOfBirth, importPlayer.DateOfBirth));
                }

                if (existing.Rating != importPlayer.Rating)
                {
                    changes.Add(new FieldChange("Rating", existing.Rating, importPlayer.Rating));
                }

                if (existing.Value != importPlayer.Value)
                {
                    changes.Add(new FieldChange("Value", existing.Value, importPlayer.Value));
                }

                if (existing.Wage != importPlayer.Wage)
                {
                    changes.Add(new FieldChange("Wage", existing.Wage, importPlayer.Wage));
                }

                if (importPlayer.ImageUrl is not null && !string.Equals(existing.ImageUrl, importPlayer.ImageUrl, StringComparison.Ordinal))
                {
                    changes.Add(new FieldChange("ImageUrl", existing.ImageUrl, importPlayer.ImageUrl));
                }

                if (importPlayer.TeamTransfermarktId is { } wantedTeamTm)
                {
                    var existingTeamTm = existing.TeamId is { } existingTeamId ? teamTransfermarktIdByExistingId.GetValueOrDefault(existingTeamId) : (int?)null;

                    if (existingTeamTm != wantedTeamTm)
                    {
                        var before = existing.TeamId is { } beforeId ? teamNameByExistingId.GetValueOrDefault(beforeId) : null;
                        var after = teamNameByTransfermarktId.GetValueOrDefault(wantedTeamTm, $"TM {wantedTeamTm}");
                        changes.Add(new FieldChange("TeamId", before, after));
                    }
                }

                if (importPlayer.NationalTeamTransfermarktId is { } wantedNationalTeamTm)
                {
                    var existingNationalTeamTm = existing.NationalTeamId is { } existingNationalTeamId ? nationalTeamTransfermarktIdByExistingId.GetValueOrDefault(existingNationalTeamId) : (int?)null;

                    if (existingNationalTeamTm != wantedNationalTeamTm)
                    {
                        var before = existing.NationalTeamId is { } beforeId ? nationalTeamNameByExistingId.GetValueOrDefault(beforeId) : null;
                        var after = nationalTeamNameByTransfermarktId.GetValueOrDefault(wantedNationalTeamTm, $"TM {wantedNationalTeamTm}");
                        changes.Add(new FieldChange("NationalTeamId", before, after));
                    }
                }

                if (changes.Count == 0)
                {
                    unchanged++;
                    continue;
                }

                writes.Add(new RowWrite(ChangeKind.Update, $"TM {importPlayer.TransfermarktId}", importPlayer.Name, changes, async context =>
                {
                    // A team or national team whose own write failed degrades to the player's current one instead
                    // of blanking it; the next run heals the link once that row has been written.
                    var teamId = importPlayer.TeamTransfermarktId is { } teamTm
                        ? context.Ids.Teams.Find(teamTm) ?? existing.TeamId
                        : existing.TeamId;

                    var nationalTeamId = importPlayer.NationalTeamTransfermarktId is { } nationalTeamTm
                        ? context.Ids.NationalTeams.Find(nationalTeamTm) ?? existing.NationalTeamId
                        : existing.NationalTeamId;

                    await context.Dispatcher.SendAsync<bool, UpdatePlayerRequest>(
                        new UpdatePlayerRequest
                        {
                            Id = existing.Id,
                            Name = importPlayer.Name,
                            DateOfBirth = importPlayer.DateOfBirth,
                            Rating = importPlayer.Rating,
                            Value = importPlayer.Value,
                            Wage = importPlayer.Wage,
                            ImageUrl = importPlayer.ImageUrl ?? existing.ImageUrl,
                            NationalTeamId = nationalTeamId,
                            TeamId = teamId,
                            TransfermarktId = importPlayer.TransfermarktId,
                        },
                        context.CancellationToken).ConfigureAwait(false);

                    return WriteOutcome.Written;
                }));
            }
            else
            {
                writes.Add(new RowWrite(ChangeKind.Create, $"TM {importPlayer.TransfermarktId}", importPlayer.Name, Array.Empty<FieldChange>(), async context =>
                {
                    // A team or national team whose own write failed leaves the new player's link null instead of
                    // failing the player; the next run heals the link once that row has been written.
                    var teamId = importPlayer.TeamTransfermarktId is { } teamTm ? context.Ids.Teams.Find(teamTm) : null;
                    var nationalTeamId = importPlayer.NationalTeamTransfermarktId is { } nationalTeamTm ? context.Ids.NationalTeams.Find(nationalTeamTm) : null;

                    var id = await context.Dispatcher.SendAsync<int, CreatePlayerRequest>(
                        new CreatePlayerRequest
                        {
                            Name = importPlayer.Name,
                            DateOfBirth = importPlayer.DateOfBirth,
                            Rating = importPlayer.Rating,
                            Value = importPlayer.Value,
                            Wage = importPlayer.Wage,
                            ImageUrl = importPlayer.ImageUrl,
                            NationalTeamId = nationalTeamId,
                            TeamId = teamId,
                            TransfermarktId = importPlayer.TransfermarktId,
                        },
                        context.CancellationToken).ConfigureAwait(false);

                    context.Ids.Players.Set(importPlayer.TransfermarktId, id);
                    return WriteOutcome.Written;
                }));
            }
        }

        return new EntityPlan("Players", writes, unchanged, Array.Empty<string>());
    }
}
