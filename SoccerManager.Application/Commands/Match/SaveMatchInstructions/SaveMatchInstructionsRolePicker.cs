using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;
using SoccerManager.Application.Services;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.Match.SaveMatchInstructions;

/// <summary>
/// Picks the four roles a new tactic row requires (the captain and the penalty, free kick and corner kick takers) from a
/// club's eleven starters.
/// </summary>
public static class SaveMatchInstructionsRolePicker
{
    /// <summary>
    /// Ranks the starters of the lineup by rating, highest first and the lower player identifier first on a tie, and gives the
    /// captaincy to the best one, a goalkeeper included. The penalty, free kick and corner kick takers are the first, second
    /// and third best outfield starters. A player without a current rating ranks as
    /// <see cref="LineupAutoPicker.DefaultRating"/>.
    /// </summary>
    /// <param name="lineupSlots">The slots of the club's lineup, with all eleven starter slots filled.</param>
    /// <param name="formationSlots">The slots of the lineup's formation, which tell the goalkeeper slots.</param>
    /// <param name="squad">The club's squad by player identifier, which gives the starters' ratings.</param>
    /// <returns>The four players.</returns>
    public static (int CaptainPlayerId, int PenaltyTakerPlayerId, int FreeKickTakerPlayerId, int CornerKickTakerPlayerId) Pick(IReadOnlyList<MatchLineupSlotDto> lineupSlots, IReadOnlyList<GetFormationSlotsResult> formationSlots, IReadOnlyDictionary<int, LineupCandidate> squad)
    {
        var goalkeeperSlots = formationSlots
            .Where(slot => slot.PositionArea == PositionArea.Goalkeeper)
            .Select(slot => slot.SlotNumber)
            .ToHashSet();

        // Reserves never take a role.
        var ranked = lineupSlots
            .Where(slot => slot.SlotNumber <= LineupAutoPicker.LastStarterSlot && slot.PlayerId.HasValue)
            .Select(slot => (PlayerId: slot.PlayerId!.Value, Rating: RatingOf(squad, slot.PlayerId.Value), IsGoalkeeper: goalkeeperSlots.Contains(slot.SlotNumber)))
            .OrderByDescending(player => player.Rating)
            .ThenBy(player => player.PlayerId)
            .ToList();

        var takers = ranked.Where(player => !player.IsGoalkeeper).ToList();

        return (ranked[0].PlayerId, takers[0].PlayerId, takers[1].PlayerId, takers[2].PlayerId);
    }

    private static int RatingOf(IReadOnlyDictionary<int, LineupCandidate> squad, int playerId)
    {
        return squad.TryGetValue(playerId, out var player) ? player.Rating ?? LineupAutoPicker.DefaultRating : LineupAutoPicker.DefaultRating;
    }
}
