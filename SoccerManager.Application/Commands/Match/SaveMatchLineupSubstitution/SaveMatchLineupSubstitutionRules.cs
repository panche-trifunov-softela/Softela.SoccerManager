using FluentValidation;
using FluentValidation.Results;
using SoccerManager.Application.Models;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.Match.SaveMatchLineupSubstitution;

/// <summary>
/// Holds the pure rules a substitution lineup save follows beyond the main lineup's slot rules: who may come on, how many, and
/// who inherits the roles of a player taken off.
/// </summary>
public static class SaveMatchLineupSubstitutionRules
{
    /// <summary>
    /// Lists the slots of a lineup's formation with the players in them: rows of any other formation are ignored, and the first
    /// of two rows in one slot wins.
    /// </summary>
    /// <param name="rows">The lineup rows to read.</param>
    /// <param name="formationId">The formation whose rows count, or <see langword="null"/> when the lineup has none.</param>
    /// <returns>The player in each filled slot, by slot number.</returns>
    public static Dictionary<int, int> SeatsOf(IEnumerable<GetMatchLineupRowsResult> rows, int? formationId)
    {
        var seats = new Dictionary<int, int>();

        foreach (var row in rows.Where(row => row.FormationId == formationId))
        {
            seats.TryAdd(row.SlotNumber, row.PlayerId);
        }

        return seats;
    }

    /// <summary>
    /// Works out the slots and players a planned save leaves: the rows the plan keeps in the target formation, with the planned
    /// inserts on top.
    /// </summary>
    /// <param name="rows">The substitution's current lineup rows.</param>
    /// <param name="formationId">The formation the save leaves the lineup in.</param>
    /// <param name="deleteRowIds">The identifiers of the rows the plan deletes.</param>
    /// <param name="inserts">The players the plan seats, by slot number.</param>
    /// <returns>The player in each filled slot after the save, by slot number.</returns>
    public static Dictionary<int, int> SeatsAfter(IReadOnlyList<GetMatchLineupRowsResult> rows, int formationId, IReadOnlyList<int> deleteRowIds, IReadOnlyDictionary<int, int> inserts)
    {
        var deleted = deleteRowIds.ToHashSet();

        var seats = SeatsOf(rows.Where(row => !deleted.Contains(row.Id)), formationId);

        foreach (var (slotNumber, playerId) in inserts)
        {
            seats[slotNumber] = playerId;
        }

        return seats;
    }

    /// <summary>
    /// Copies the squad for the lineup planner with the substitution's own players marked as not suspended. The planner checks
    /// both slots of a swap, so without this a suspended starter could never be taken off; bringing a suspended player on is
    /// refused by <see cref="EnsureAllowed"/> instead.
    /// </summary>
    /// <param name="squad">The club's squad, by player identifier.</param>
    /// <param name="playerIds">The identifiers of the players already in the substitution's lineup.</param>
    /// <returns>The squad with those players not suspended.</returns>
    public static IReadOnlyDictionary<int, LineupCandidate> ToPlanSquad(IReadOnlyDictionary<int, LineupCandidate> squad, IEnumerable<int> playerIds)
    {
        var inSubstitution = playerIds.ToHashSet();

        return squad.ToDictionary(
            entry => entry.Key,
            entry => inSubstitution.Contains(entry.Key) ? entry.Value with { IsSuspended = false } : entry.Value);
    }

    /// <summary>
    /// Refuses a save that breaks a substitution rule: a player who comes into the lineup or the starting eleven must be in the
    /// main lineup and, for the starting eleven, not suspended; the starting eleven may not shrink; and it may hold at most
    /// <see cref="MatchSubstitutionLimits.MaxPlayersBroughtOn"/> players who are not main starters, unless the save does not add
    /// to them.
    /// </summary>
    /// <param name="before">The substitution's players by slot before the save.</param>
    /// <param name="after">The substitution's players by slot after the save.</param>
    /// <param name="mainRows">The club's main lineup rows.</param>
    /// <param name="squad">The club's squad, by player identifier, with the true suspensions.</param>
    /// <exception cref="ValidationException">Thrown when the save breaks one of the rules above.</exception>
    public static void EnsureAllowed(IReadOnlyDictionary<int, int> before, IReadOnlyDictionary<int, int> after, IReadOnlyList<GetMatchLineupRowsResult> mainRows, IReadOnlyDictionary<int, LineupCandidate> squad)
    {
        var mainPlayers = mainRows.Select(row => row.PlayerId).ToHashSet();

        var mainStarters = mainRows
            .Where(row => row.SlotNumber <= LineupAutoPicker.LastStarterSlot)
            .Select(row => row.PlayerId)
            .ToHashSet();

        var playersBefore = before.Values.ToHashSet();
        var startersBefore = StartersOf(before);
        var startersAfter = StartersOf(after);

        foreach (var (slotNumber, playerId) in after.OrderBy(seat => seat.Key))
        {
            var comesIntoLineup = !playersBefore.Contains(playerId);
            var comesOn = slotNumber <= LineupAutoPicker.LastStarterSlot && !startersBefore.Contains(playerId);

            if ((comesIntoLineup || comesOn) && !mainPlayers.Contains(playerId))
            {
                throw Fail("PlayerId", $"Slot {slotNumber}: only players in the main lineup can come on.");
            }

            if (comesOn && squad.TryGetValue(playerId, out var candidate) && candidate.IsSuspended)
            {
                throw Fail("PlayerId", $"Slot {slotNumber}: a suspended player can't come on.");
            }
        }

        if (startersAfter.Count < startersBefore.Count)
        {
            throw Fail("SlotNumber", "Keep the starting XI full: swap a starter with a bench player.");
        }

        var broughtOnBefore = startersBefore.Count(playerId => !mainStarters.Contains(playerId));
        var broughtOnAfter = startersAfter.Count(playerId => !mainStarters.Contains(playerId));

        // A main lineup change can leave a saved substitution above the limit, so only a save that adds to it is refused.
        if (broughtOnAfter > MatchSubstitutionLimits.MaxPlayersBroughtOn && broughtOnAfter > broughtOnBefore)
        {
            throw Fail("PlayerId", $"A situation can bring on at most {MatchSubstitutionLimits.MaxPlayersBroughtOn} players.");
        }
    }

    /// <summary>
    /// Works out the four roles after a save: a role whose holder the save took off the pitch goes to the player it brought on,
    /// when it brought on exactly one; every other role stays with its holder.
    /// </summary>
    /// <param name="tactic">The substitution's tactic, which holds the roles before the save.</param>
    /// <param name="before">The substitution's players by slot before the save.</param>
    /// <param name="after">The substitution's players by slot after the save.</param>
    /// <returns>The captain and the penalty, free kick and corner kick takers after the save.</returns>
    public static (int CaptainPlayerId, int PenaltyTakerPlayerId, int FreeKickTakerPlayerId, int CornerKickTakerPlayerId) MoveRoles(SoccerManager.Domain.Entities.MatchTeamTactic tactic, IReadOnlyDictionary<int, int> before, IReadOnlyDictionary<int, int> after)
    {
        var startersBefore = StartersOf(before);
        var startersAfter = StartersOf(after);

        var broughtOn = startersAfter.Where(playerId => !startersBefore.Contains(playerId)).ToList();

        int Holder(int playerId) =>
            broughtOn.Count == 1 && startersBefore.Contains(playerId) && !startersAfter.Contains(playerId) ? broughtOn[0] : playerId;

        return (Holder(tactic.CaptainPlayerId), Holder(tactic.PenaltyTakerPlayerId), Holder(tactic.FreeKickTakerPlayerId), Holder(tactic.CornerKickTakerPlayerId));
    }

    // The players in the starter slots, which are the ones the roles can sit with.
    private static HashSet<int> StartersOf(IReadOnlyDictionary<int, int> seats)
    {
        return seats
            .Where(seat => seat.Key <= LineupAutoPicker.LastStarterSlot)
            .Select(seat => seat.Value)
            .ToHashSet();
    }

    // The same exception the validators raise, so a refused save answers with the same 400 body.
    private static ValidationException Fail(string propertyName, string message)
    {
        return new ValidationException(new[]
        {
            new ValidationFailure(propertyName, message),
        });
    }
}
