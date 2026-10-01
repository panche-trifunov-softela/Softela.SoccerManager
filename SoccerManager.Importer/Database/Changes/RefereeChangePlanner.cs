using SoccerManager.Application.Commands.Referee.CreateReferee;
using SoccerManager.Application.Dtos;
using SoccerManager.Domain.Enums;
using SoccerManager.Importer.Transform.Model;

namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// Plans the referee writes: create-only, like positions, so a matched referee is used exactly as it already is.
/// </summary>
public static class RefereeChangePlanner
{
    /// <summary>
    /// Builds the referees plan.
    /// </summary>
    /// <param name="importReferees">The import model's referees.</param>
    /// <param name="existingReferees">The database's existing referees.</param>
    /// <returns>The referees entity plan.</returns>
    public static EntityPlan Plan(IReadOnlyList<ImportReferee> importReferees, IReadOnlyList<RefereeDto> existingReferees)
    {
        var notes = new List<string>();
        var writes = new List<RowWrite>();
        var unchanged = 0;

        var existingByName = ExistingRowMatcher.MatchByKey(
            existingReferees,
            referee => referee.Name,
            referee => referee.Id,
            notes);

        foreach (var importReferee in importReferees)
        {
            if (existingByName.ContainsKey(importReferee.Name))
            {
                unchanged++;
                continue;
            }

            writes.Add(new RowWrite(ChangeKind.Create, importReferee.Name, importReferee.Name, Array.Empty<FieldChange>(), async context =>
            {
                await context.Dispatcher.SendAsync<int, CreateRefereeRequest>(
                    new CreateRefereeRequest { Name = importReferee.Name, ImageUrl = null, Tolerance = Tolerance.Balanced },
                    context.CancellationToken).ConfigureAwait(false);

                return WriteOutcome.Written;
            }));
        }

        return new EntityPlan("Referees", writes, unchanged, notes);
    }
}
