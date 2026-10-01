using SoccerManager.Application.Commands.PlayerPosition.CreatePlayerPosition;
using SoccerManager.Application.Commands.PlayerPosition.UpdatePlayerPosition;
using SoccerManager.Application.Dtos;
using SoccerManager.Importer.Transform.Model;

namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// Plans the player position writes: matched by (player, position name) rather than by database id, since the
/// position itself is matched by name. An existing row whose name is not in the player's import list is kept, not
/// deleted, and counted in one note.
/// </summary>
public static class PlayerPositionChangePlanner
{
    /// <summary>
    /// Builds the player positions plan. Unlike the other planners, this one does not seed <see cref="ImportIds"/>:
    /// a player position links two rows that are themselves matched elsewhere, so it only ever reads
    /// <see cref="ImportIds.Players"/> and <see cref="ImportIds.Positions"/>, at write time.
    /// </summary>
    /// <param name="importPlayers">The import model's players, each with its positions.</param>
    /// <param name="existingPlayers">The database's existing players, used to find a matched player's database id.</param>
    /// <param name="existingPositions">The database's existing positions, used to map a position id back to its name.</param>
    /// <param name="playerPositionsByPlayerId">The existing position ratings of every database player the import model references, keyed by database player id.</param>
    /// <returns>The player positions entity plan.</returns>
    public static EntityPlan Plan(
        IReadOnlyList<ImportPlayer> importPlayers,
        IReadOnlyList<PlayerDto> existingPlayers,
        IReadOnlyList<PositionDto> existingPositions,
        IReadOnlyDictionary<int, IReadOnlyList<PlayerPositionDto>> playerPositionsByPlayerId)
    {
        var writes = new List<RowWrite>();
        var unchanged = 0;
        var keptExistingCount = 0;

        var existingByTransfermarktId = existingPlayers
            .Where(player => player.TransfermarktId.HasValue)
            .ToDictionary(player => player.TransfermarktId!.Value, player => player);

        var positionNameByExistingId = existingPositions.ToDictionary(position => position.Id, position => position.Name);

        foreach (var importPlayer in importPlayers)
        {
            var hasExistingPlayer = existingByTransfermarktId.TryGetValue(importPlayer.TransfermarktId, out var existingPlayer);

            var existingRowsByName = hasExistingPlayer
                ? MatchExistingRowsByPositionName(playerPositionsByPlayerId.GetValueOrDefault(existingPlayer!.Id) ?? Array.Empty<PlayerPositionDto>(), positionNameByExistingId)
                : new Dictionary<string, PlayerPositionDto>(StringComparer.Ordinal);

            var importPositionNames = new HashSet<string>(importPlayer.Positions.Select(position => position.PositionName), StringComparer.Ordinal);

            foreach (var importPosition in importPlayer.Positions)
            {
                if (existingRowsByName.TryGetValue(importPosition.PositionName, out var existingRow))
                {
                    if (existingRow.Quality == importPosition.Quality)
                    {
                        unchanged++;
                        continue;
                    }

                    var changes = new[] { new FieldChange("Quality", existingRow.Quality, importPosition.Quality) };
                    var quality = importPosition.Quality;

                    writes.Add(new RowWrite(
                        ChangeKind.Update,
                        $"TM {importPlayer.TransfermarktId} {importPosition.PositionName}",
                        importPlayer.Name,
                        changes,
                        async context =>
                        {
                            await context.Dispatcher.SendAsync<bool, UpdatePlayerPositionRequest>(
                                new UpdatePlayerPositionRequest { Id = existingRow.Id, PlayerId = existingRow.PlayerId, PositionId = existingRow.PositionId, Quality = quality },
                                context.CancellationToken).ConfigureAwait(false);

                            return WriteOutcome.Written;
                        }));

                    continue;
                }

                var positionName = importPosition.PositionName;
                var createQuality = importPosition.Quality;
                var transfermarktId = importPlayer.TransfermarktId;
                var knownPlayerId = hasExistingPlayer ? existingPlayer!.Id : (int?)null;

                writes.Add(new RowWrite(
                    ChangeKind.Create,
                    $"TM {transfermarktId} {positionName}",
                    importPlayer.Name,
                    Array.Empty<FieldChange>(),
                    async context =>
                    {
                        // For a matched player the id is already known; for a brand-new player it only exists once
                        // that player's own create has completed, so it is read from the ids written so far.
                        var playerId = knownPlayerId ?? context.Ids.Players.Find(transfermarktId);
                        if (playerId is null)
                        {
                            return WriteOutcome.Skipped($"player TM {transfermarktId} was not written");
                        }

                        var positionId = context.Ids.Positions.Find(positionName);
                        if (positionId is null)
                        {
                            return WriteOutcome.Skipped($"position '{positionName}' was not written");
                        }

                        await context.Dispatcher.SendAsync<int, CreatePlayerPositionRequest>(
                            new CreatePlayerPositionRequest { PlayerId = playerId.Value, PositionId = positionId.Value, Quality = createQuality },
                            context.CancellationToken).ConfigureAwait(false);

                        return WriteOutcome.Written;
                    }));
            }

            if (hasExistingPlayer)
            {
                keptExistingCount += existingRowsByName.Keys.Count(name => !importPositionNames.Contains(name));
            }
        }

        var notes = keptExistingCount > 0
            ? new List<string> { $"{keptExistingCount} existing player position(s) not present in the import data were kept." }
            : new List<string>();

        return new EntityPlan("Player positions", writes, unchanged, notes);
    }

    // Groups a player's existing position ratings by position name, keeping the lowest-id row when two rows
    // happen to share a name (only possible if the positions table itself has a duplicate-name row, which the
    // positions plan already reports on its own).
    private static Dictionary<string, PlayerPositionDto> MatchExistingRowsByPositionName(
        IReadOnlyList<PlayerPositionDto> rows, IReadOnlyDictionary<int, string> positionNameByExistingId)
    {
        var result = new Dictionary<string, PlayerPositionDto>(StringComparer.Ordinal);

        foreach (var group in rows.GroupBy(row => positionNameByExistingId.GetValueOrDefault(row.PositionId, $"PositionId {row.PositionId}"), StringComparer.Ordinal))
        {
            result[group.Key] = group.OrderBy(row => row.Id).First();
        }

        return result;
    }
}
