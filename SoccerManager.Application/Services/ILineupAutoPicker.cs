using SoccerManager.Application.Models;

namespace SoccerManager.Application.Services;

/// <summary>
/// Picks a lineup from a squad by one deterministic rule, so the auto pick and the re-seat after a formation change agree.
/// </summary>
public interface ILineupAutoPicker
{
    /// <summary>
    /// Picks the whole lineup for a formation: the eleven starters, then the bench.
    /// </summary>
    /// <param name="slots">The slots of the formation.</param>
    /// <param name="squad">The squad to pick from; suspended players and players without position data are skipped.</param>
    /// <returns>The picked players' identifiers by slot number; slots no eligible player is left for are absent.</returns>
    IReadOnlyDictionary<int, int> Pick(IReadOnlyList<GetFormationSlotsResult> slots, IEnumerable<LineupCandidate> squad);

    /// <summary>
    /// Seats players into open starter slots: the goalkeeper slot first, then the best remaining pairs.
    /// </summary>
    /// <param name="openSlots">The starter slots to fill.</param>
    /// <param name="players">The players to seat.</param>
    /// <returns>The seated players' identifiers by slot number; a slot or player left over is absent.</returns>
    IReadOnlyDictionary<int, int> AssignStarters(IReadOnlyList<GetFormationSlotsResult> openSlots, IReadOnlyList<LineupCandidate> players);
}
