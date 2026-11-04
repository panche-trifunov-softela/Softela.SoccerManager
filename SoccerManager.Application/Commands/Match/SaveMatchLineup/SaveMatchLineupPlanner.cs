using FluentValidation;
using FluentValidation.Results;
using SoccerManager.Application.Models;
using SoccerManager.Application.Services;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.Match.SaveMatchLineup;

/// <summary>
/// Works out, from a save request and the stored lineup, which rows to delete and insert, applying every lineup rule.
/// </summary>
public static class SaveMatchLineupPlanner
{
    /// <summary>
    /// Plans a save: re-seats the stored lineup when the formation changes, puts the request's player in the request's slot
    /// on top, and checks every player the save writes. The lineup is the rows of the stored formation; rows of another
    /// formation, which only the removed per-slot endpoints may have left, are ignored and left alone unless the formation
    /// changes, which deletes every row.
    /// </summary>
    /// <param name="request">The save request.</param>
    /// <param name="rows">Every stored row of the club for the match, in slot order.</param>
    /// <param name="storedFormationId">The formation of the lineup, which is the formation of the first row, or <see langword="null"/> when there are no rows.</param>
    /// <param name="formationId">The target formation: the request's when it names one, otherwise the stored one.</param>
    /// <param name="slots">The slots of the target formation.</param>
    /// <param name="squad">The club's squad by player identifier.</param>
    /// <param name="picker">The picker that seats starters who have no slot of their position after a formation change.</param>
    /// <returns>The identifiers of the stored rows to delete, and the players to insert by slot number.</returns>
    /// <exception cref="ValidationException">Thrown when a slot or player breaks a lineup rule; the first failure found is thrown.</exception>
    public static (IReadOnlyList<int> DeleteRowIds, IReadOnlyDictionary<int, int> Inserts) Plan(SaveMatchLineupRequest request, IReadOnlyList<GetMatchLineupRowsResult> rows, int? storedFormationId, int formationId, IReadOnlyList<GetFormationSlotsResult> slots, IReadOnlyDictionary<int, LineupCandidate> squad, ILineupAutoPicker picker)
    {
        var lineupRows = rows.Where(row => row.FormationId == storedFormationId).ToList();
        var slotNumbers = slots.Select(slot => slot.SlotNumber).ToHashSet();
        var switched = storedFormationId is not null && formationId != storedFormationId;

        var target = switched
            ? Reseat(lineupRows, slots, slotNumbers, squad, picker)
            : lineupRows.ToDictionary(row => row.SlotNumber, row => row.PlayerId);

        var before = new Dictionary<int, int>(target);

        if (request.SlotNumber is int slotNumber)
        {
            if (!slotNumbers.Contains(slotNumber))
            {
                throw Fail($"Slot {slotNumber}: the formation has no slot {slotNumber}.");
            }

            PutPlayerInSlot(target, slotNumber, request.PlayerId);
        }

        var changed = new SortedSet<int>(before.Keys.Union(target.Keys).Where(number => HasChanged(before, target, number)));

        foreach (var number in changed)
        {
            if (target.TryGetValue(number, out var playerId))
            {
                EnsureEligible(number, playerId, squad);
            }
        }

        if (switched)
        {
            var deleteAll = rows.Select(row => row.Id).ToList();

            return (deleteAll, new SortedDictionary<int, int>(target));
        }

        var deleteRowIds = lineupRows
            .Where(row => changed.Contains(row.SlotNumber))
            .Select(row => row.Id)
            .ToList();

        var inserts = new SortedDictionary<int, int>();

        foreach (var number in changed.Where(target.ContainsKey))
        {
            inserts[number] = target[number];
        }

        return (deleteRowIds, inserts);
    }

    private static void PutPlayerInSlot(Dictionary<int, int> target, int slotNumber, int? playerId)
    {
        if (playerId is not int newPlayerId)
        {
            target.Remove(slotNumber);

            return;
        }

        var occupied = target.TryGetValue(slotNumber, out var occupant);
        var previousSlot = target.Where(entry => entry.Value == newPlayerId).Select(entry => (int?)entry.Key).FirstOrDefault();

        if (previousSlot == slotNumber)
        {
            return;
        }

        // The player already in the lineup is moved: the player the slot held takes his old slot (a swap), or it empties (a move).
        if (previousSlot is int otherSlot)
        {
            if (occupied)
            {
                target[otherSlot] = occupant;
            }
            else
            {
                target.Remove(otherSlot);
            }
        }

        target[slotNumber] = newPlayerId;
    }

    private static Dictionary<int, int> Reseat(IReadOnlyList<GetMatchLineupRowsResult> lineupRows, IReadOnlyList<GetFormationSlotsResult> slots, HashSet<int> slotNumbers, IReadOnlyDictionary<int, LineupCandidate> squad, ILineupAutoPicker picker)
    {
        var currentRows = lineupRows.OrderBy(row => row.SlotNumber).ThenBy(row => row.Id).ToList();

        // A refusal names the player's current slot, which is the one the user sees on screen.
        foreach (var row in currentRows)
        {
            EnsureEligible(row.SlotNumber, row.PlayerId, squad);
        }

        var currentStarters = currentRows.Where(row => row.SlotNumber <= LineupAutoPicker.LastStarterSlot).ToList();
        var newStarterSlots = slots.Where(slot => slot.SlotNumber <= LineupAutoPicker.LastStarterSlot).OrderBy(slot => slot.SlotNumber).ToList();

        var seat = new Dictionary<int, int>();
        var seated = new HashSet<int>();

        // Pass 1: a starter keeps his place when the new formation has a slot for the position he held.
        foreach (var newSlot in newStarterSlots)
        {
            var sameRole = currentStarters.FirstOrDefault(row => !seated.Contains(row.PlayerId) && row.SlotPositionId == newSlot.PositionId);

            if (sameRole is not null)
            {
                seat[newSlot.SlotNumber] = sameRole.PlayerId;
                seated.Add(sameRole.PlayerId);
            }
        }

        // Pass 2: the starters left over fill the open slots. The goalkeeper slot takes only a goalkeeper, so an outfielder is never put in goal.
        var open = newStarterSlots.Where(slot => !seat.ContainsKey(slot.SlotNumber)).ToList();
        var left = currentStarters.Where(row => !seated.Contains(row.PlayerId)).Select(row => squad[row.PlayerId]).ToList();

        var goalkeeperSlot = open.FirstOrDefault(slot => slot.PositionArea == PositionArea.Goalkeeper);

        if (goalkeeperSlot is not null)
        {
            open.Remove(goalkeeperSlot);

            var keepers = left.Where(player => player.Positions.Any(position => position.IsGoalkeeper)).ToList();

            if (keepers.Count > 0)
            {
                foreach (var (slotNumber, playerId) in picker.AssignStarters([goalkeeperSlot], keepers))
                {
                    seat[slotNumber] = playerId;
                    left.RemoveAll(player => player.PlayerId == playerId);
                }
            }
        }

        foreach (var (slotNumber, playerId) in picker.AssignStarters(open, left))
        {
            seat[slotNumber] = playerId;
        }

        // The bench keeps its numbers; a number the new formation lacks cannot be kept.
        var benchRows = currentRows.Where(row => row.SlotNumber >= LineupAutoPicker.FirstBenchSlot);

        foreach (var benchRow in benchRows)
        {
            if (!slotNumbers.Contains(benchRow.SlotNumber))
            {
                throw Fail($"Slot {benchRow.SlotNumber}: the formation has no slot {benchRow.SlotNumber}.");
            }

            seat[benchRow.SlotNumber] = benchRow.PlayerId;
        }

        return seat;
    }

    private static bool HasChanged(Dictionary<int, int> before, Dictionary<int, int> target, int number)
    {
        if (!before.TryGetValue(number, out var oldPlayer))
        {
            return target.ContainsKey(number);
        }

        return !target.TryGetValue(number, out var newPlayer) || newPlayer != oldPlayer;
    }

    private static void EnsureEligible(int slotNumber, int playerId, IReadOnlyDictionary<int, LineupCandidate> squad)
    {
        if (!squad.TryGetValue(playerId, out var candidate))
        {
            throw Fail($"Slot {slotNumber}: the player is not in this club's squad.");
        }

        if (!candidate.HasPositionData)
        {
            throw Fail($"Slot {slotNumber}: the player has no position data.");
        }

        if (candidate.IsSuspended)
        {
            throw Fail($"Slot {slotNumber}: the player is suspended.");
        }
    }

    private static ValidationException Fail(string message)
    {
        return new ValidationException(new[]
        {
            new ValidationFailure("SlotNumber", message),
        });
    }
}
