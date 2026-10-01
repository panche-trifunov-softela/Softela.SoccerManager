using SoccerManager.Application.Commands.Team.CreateTeam;
using SoccerManager.Application.Commands.Team.UpdateTeam;
using SoccerManager.Application.Dtos;
using SoccerManager.Importer.Transform;
using SoccerManager.Importer.Transform.Model;

namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// Plans the team writes: matched by Transfermarkt id, with a stadium link compared by stadium key rather than by
/// database id, so a duplicate-name stadium row never fakes a change and a real move still shows one.
/// </summary>
public static class TeamChangePlanner
{
    /// <summary>
    /// Builds the teams plan, seeding <paramref name="ids"/> with the database id matched for every team Transfermarkt id.
    /// </summary>
    /// <param name="importTeams">The import model's teams.</param>
    /// <param name="existingTeams">The database's existing teams.</param>
    /// <param name="importStadiums">The import model's stadiums, used to display a stadium link by name.</param>
    /// <param name="existingStadiums">The database's existing stadiums, used to map an existing team's StadiumId back to a stadium key.</param>
    /// <param name="ids">The id lookups to read stadium ids from and to seed <see cref="ImportIds.Teams"/> on.</param>
    /// <returns>The teams entity plan.</returns>
    public static EntityPlan Plan(
        IReadOnlyList<ImportTeam> importTeams,
        IReadOnlyList<TeamDto> existingTeams,
        IReadOnlyList<ImportStadium> importStadiums,
        IReadOnlyList<StadiumDto> existingStadiums,
        ImportIds ids)
    {
        var writes = new List<RowWrite>();
        var unchanged = 0;

        var existingByTransfermarktId = existingTeams
            .Where(team => team.TransfermarktId.HasValue)
            .ToDictionary(team => team.TransfermarktId!.Value, team => team);

        var stadiumKeyByExistingId = existingStadiums.ToDictionary(stadium => stadium.Id, stadium => NameNormalizer.Normalize(stadium.Name));
        var stadiumNameByExistingId = existingStadiums.ToDictionary(stadium => stadium.Id, stadium => stadium.Name);
        var stadiumNameByKey = importStadiums.ToDictionary(stadium => stadium.Key, stadium => stadium.Name, StringComparer.Ordinal);

        foreach (var importTeam in importTeams)
        {
            if (existingByTransfermarktId.TryGetValue(importTeam.TransfermarktId, out var existing))
            {
                ids.Teams.Set(importTeam.TransfermarktId, existing.Id);

                var changes = new List<FieldChange>();

                if (!string.Equals(existing.Name, importTeam.Name, StringComparison.Ordinal))
                {
                    changes.Add(new FieldChange("Name", existing.Name, importTeam.Name));
                }

                if (existing.FinancialState != importTeam.FinancialState)
                {
                    changes.Add(new FieldChange("FinancialState", existing.FinancialState, importTeam.FinancialState));
                }

                if (importTeam.StadiumKey is not null)
                {
                    var existingStadiumKey = existing.StadiumId is { } existingStadiumId ? stadiumKeyByExistingId.GetValueOrDefault(existingStadiumId) : null;

                    if (!string.Equals(existingStadiumKey, importTeam.StadiumKey, StringComparison.Ordinal))
                    {
                        var before = existing.StadiumId is { } beforeId ? stadiumNameByExistingId.GetValueOrDefault(beforeId) : null;
                        var after = stadiumNameByKey.GetValueOrDefault(importTeam.StadiumKey, importTeam.StadiumKey);
                        changes.Add(new FieldChange("StadiumId", before, after));
                    }
                }

                if (changes.Count == 0)
                {
                    unchanged++;
                    continue;
                }

                writes.Add(new RowWrite(ChangeKind.Update, $"TM {importTeam.TransfermarktId}", importTeam.Name, changes, async context =>
                {
                    // A stadium whose own write failed degrades to the team's current stadium instead of blanking
                    // it; the next run heals the link once the stadium itself has been written.
                    var stadiumId = importTeam.StadiumKey is null
                        ? existing.StadiumId
                        : context.Ids.Stadiums.Find(importTeam.StadiumKey) ?? existing.StadiumId;

                    await context.Dispatcher.SendAsync<bool, UpdateTeamRequest>(
                        new UpdateTeamRequest
                        {
                            Id = existing.Id,
                            Name = importTeam.Name,
                            StadiumId = stadiumId,
                            FinancialState = importTeam.FinancialState,
                            JerseyUrl = existing.JerseyUrl,
                            LogoUrl = existing.LogoUrl,
                            TransfermarktId = importTeam.TransfermarktId,
                        },
                        context.CancellationToken).ConfigureAwait(false);

                    return WriteOutcome.Written;
                }));
            }
            else
            {
                writes.Add(new RowWrite(ChangeKind.Create, $"TM {importTeam.TransfermarktId}", importTeam.Name, Array.Empty<FieldChange>(), async context =>
                {
                    var stadiumId = importTeam.StadiumKey is null ? null : context.Ids.Stadiums.Find(importTeam.StadiumKey);

                    var id = await context.Dispatcher.SendAsync<int, CreateTeamRequest>(
                        new CreateTeamRequest
                        {
                            Name = importTeam.Name,
                            StadiumId = stadiumId,
                            FinancialState = importTeam.FinancialState,
                            JerseyUrl = null,
                            LogoUrl = null,
                            TransfermarktId = importTeam.TransfermarktId,
                        },
                        context.CancellationToken).ConfigureAwait(false);

                    context.Ids.Teams.Set(importTeam.TransfermarktId, id);
                    return WriteOutcome.Written;
                }));
            }
        }

        return new EntityPlan("Teams", writes, unchanged, Array.Empty<string>());
    }
}
