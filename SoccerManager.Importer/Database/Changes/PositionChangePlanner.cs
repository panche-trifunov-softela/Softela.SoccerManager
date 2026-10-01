using SoccerManager.Application.Commands.Position.CreatePosition;
using SoccerManager.Application.Dtos;
using SoccerManager.Importer.Transform.Model;

namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// Plans the position writes: the 13-position catalog is create-only, since a position's shape never changes once
/// seeded, so a match is left exactly as it is even when its area or side disagrees with the catalog.
/// </summary>
public static class PositionChangePlanner
{
    /// <summary>
    /// Builds the positions plan, seeding <paramref name="ids"/> with the database id matched for every position name.
    /// </summary>
    /// <param name="catalog">The 13 recognized positions.</param>
    /// <param name="existingPositions">The database's existing positions.</param>
    /// <param name="ids">The id lookups to seed <see cref="ImportIds.Positions"/> on.</param>
    /// <returns>The positions entity plan.</returns>
    public static EntityPlan Plan(IReadOnlyList<ImportPosition> catalog, IReadOnlyList<PositionDto> existingPositions, ImportIds ids)
    {
        var notes = new List<string>();
        var writes = new List<RowWrite>();
        var unchanged = 0;

        var existingByName = ExistingRowMatcher.MatchByKey(
            existingPositions,
            position => position.Name,
            position => position.Id,
            notes);

        foreach (var position in catalog)
        {
            if (existingByName.TryGetValue(position.Name, out var existing))
            {
                ids.Positions.Set(position.Name, existing.Id);
                unchanged++;

                if (existing.Area != position.Area || existing.Side != position.Side)
                {
                    notes.Add(
                        $"Position '{position.Name}' exists with Area={existing.Area}, Side={existing.Side}; " +
                        $"the catalog expects Area={position.Area}, Side={position.Side}. Left unchanged.");
                }

                continue;
            }

            writes.Add(new RowWrite(ChangeKind.Create, position.Name, position.Name, Array.Empty<FieldChange>(), async context =>
            {
                var id = await context.Dispatcher.SendAsync<int, CreatePositionRequest>(
                    new CreatePositionRequest { Name = position.Name, Area = position.Area, Side = position.Side },
                    context.CancellationToken).ConfigureAwait(false);

                context.Ids.Positions.Set(position.Name, id);
                return WriteOutcome.Written;
            }));
        }

        return new EntityPlan("Positions", writes, unchanged, notes);
    }
}
