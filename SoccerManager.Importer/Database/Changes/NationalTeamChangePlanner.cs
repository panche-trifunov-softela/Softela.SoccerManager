using SoccerManager.Application.Commands.NationalTeam.CreateNationalTeam;
using SoccerManager.Application.Commands.NationalTeam.UpdateNationalTeam;
using SoccerManager.Application.Dtos;
using SoccerManager.Importer.Transform.Model;

namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// Plans the national team writes: matched by Transfermarkt id. Unlike a club, the dataset carries no stadium for
/// a national team, so that field is always copied from the existing row on update and left null on create.
/// </summary>
public static class NationalTeamChangePlanner
{
    /// <summary>
    /// Builds the national teams plan, seeding <paramref name="ids"/> with the database id matched for every Transfermarkt id.
    /// </summary>
    /// <param name="importNationalTeams">The import model's national teams.</param>
    /// <param name="existingNationalTeams">The database's existing national teams.</param>
    /// <param name="ids">The id lookups to seed <see cref="ImportIds.NationalTeams"/> on.</param>
    /// <returns>The national teams entity plan.</returns>
    public static EntityPlan Plan(IReadOnlyList<ImportNationalTeam> importNationalTeams, IReadOnlyList<NationalTeamDto> existingNationalTeams, ImportIds ids)
    {
        var writes = new List<RowWrite>();
        var unchanged = 0;

        var existingByTransfermarktId = existingNationalTeams
            .Where(team => team.TransfermarktId.HasValue)
            .ToDictionary(team => team.TransfermarktId!.Value, team => team);

        foreach (var importTeam in importNationalTeams)
        {
            if (existingByTransfermarktId.TryGetValue(importTeam.TransfermarktId, out var existing))
            {
                ids.NationalTeams.Set(importTeam.TransfermarktId, existing.Id);

                var changes = new List<FieldChange>();

                if (!string.Equals(existing.Name, importTeam.Name, StringComparison.Ordinal))
                {
                    changes.Add(new FieldChange("Name", existing.Name, importTeam.Name));
                }

                if (importTeam.LogoUrl is not null && !string.Equals(existing.LogoUrl, importTeam.LogoUrl, StringComparison.Ordinal))
                {
                    changes.Add(new FieldChange("LogoUrl", existing.LogoUrl, importTeam.LogoUrl));
                }

                if (changes.Count == 0)
                {
                    unchanged++;
                    continue;
                }

                writes.Add(new RowWrite(ChangeKind.Update, $"TM {importTeam.TransfermarktId}", importTeam.Name, changes, async context =>
                {
                    await context.Dispatcher.SendAsync<bool, UpdateNationalTeamRequest>(
                        new UpdateNationalTeamRequest
                        {
                            Id = existing.Id,
                            Name = importTeam.Name,
                            StadiumId = existing.StadiumId,
                            JerseyUrl = existing.JerseyUrl,
                            LogoUrl = importTeam.LogoUrl ?? existing.LogoUrl,
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
                    var id = await context.Dispatcher.SendAsync<int, CreateNationalTeamRequest>(
                        new CreateNationalTeamRequest
                        {
                            Name = importTeam.Name,
                            StadiumId = null,
                            JerseyUrl = null,
                            LogoUrl = importTeam.LogoUrl,
                            TransfermarktId = importTeam.TransfermarktId,
                        },
                        context.CancellationToken).ConfigureAwait(false);

                    context.Ids.NationalTeams.Set(importTeam.TransfermarktId, id);
                    return WriteOutcome.Written;
                }));
            }
        }

        return new EntityPlan("National teams", writes, unchanged, Array.Empty<string>());
    }
}
