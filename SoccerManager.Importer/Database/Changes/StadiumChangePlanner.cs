using SoccerManager.Application.Commands.Stadium.CreateStadium;
using SoccerManager.Application.Commands.Stadium.UpdateStadium;
using SoccerManager.Application.Dtos;
using SoccerManager.Importer.Transform;
using SoccerManager.Importer.Transform.Model;

namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// Plans the stadium writes: a stadium is matched by its normalized name, the same key the transform already
/// merges shared grounds by, so "Estádio" and "Estadio" land on one row instead of two.
/// </summary>
public static class StadiumChangePlanner
{
    /// <summary>
    /// Builds the stadiums plan, seeding <paramref name="ids"/> with the database id matched for every stadium key.
    /// </summary>
    /// <param name="importStadiums">The import model's stadiums.</param>
    /// <param name="existingStadiums">The database's existing stadiums.</param>
    /// <param name="ids">The id lookups to seed <see cref="ImportIds.Stadiums"/> on.</param>
    /// <returns>The stadiums entity plan.</returns>
    public static EntityPlan Plan(IReadOnlyList<ImportStadium> importStadiums, IReadOnlyList<StadiumDto> existingStadiums, ImportIds ids)
    {
        var notes = new List<string>();
        var writes = new List<RowWrite>();
        var unchanged = 0;

        var existingByKey = ExistingRowMatcher.MatchByKey(
            existingStadiums,
            stadium => NameNormalizer.Normalize(stadium.Name),
            stadium => stadium.Id,
            notes);

        foreach (var importStadium in importStadiums)
        {
            if (existingByKey.TryGetValue(importStadium.Key, out var existing))
            {
                ids.Stadiums.Set(importStadium.Key, existing.Id);

                if (existing.Size == importStadium.Size)
                {
                    unchanged++;
                    continue;
                }

                var changes = new[] { new FieldChange("Size", existing.Size, importStadium.Size) };

                writes.Add(new RowWrite(ChangeKind.Update, existing.Name, existing.Name, changes, async context =>
                {
                    // The database spelling of the name and its image are kept: the dataset only ever supplies a
                    // size, never a reason to overwrite either.
                    await context.Dispatcher.SendAsync<bool, UpdateStadiumRequest>(
                        new UpdateStadiumRequest { Id = existing.Id, Name = existing.Name, ImageUrl = existing.ImageUrl, Size = importStadium.Size },
                        context.CancellationToken).ConfigureAwait(false);

                    return WriteOutcome.Written;
                }));
            }
            else
            {
                writes.Add(new RowWrite(ChangeKind.Create, importStadium.Name, importStadium.Name, Array.Empty<FieldChange>(), async context =>
                {
                    var id = await context.Dispatcher.SendAsync<int, CreateStadiumRequest>(
                        new CreateStadiumRequest { Name = importStadium.Name, ImageUrl = null, Size = importStadium.Size },
                        context.CancellationToken).ConfigureAwait(false);

                    context.Ids.Stadiums.Set(importStadium.Key, id);
                    return WriteOutcome.Written;
                }));
            }
        }

        return new EntityPlan("Stadiums", writes, unchanged, notes);
    }
}
