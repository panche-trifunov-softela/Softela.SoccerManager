using SoccerManager.Application.Models;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Services;

/// <summary>
/// Picks a lineup from a squad with integer arithmetic only, so the same squad always gives the same lineup. A player's score
/// for a slot is his rating times how well he fits the slot times his condition factor; ties go to the higher rating, then the
/// lower squad number, then the lower player identifier.
/// </summary>
public sealed class LineupAutoPicker : ILineupAutoPicker
{
    /// <summary>
    /// The name of the formation preferred when nothing names one.
    /// </summary>
    public const string DefaultFormationName = "4-4-2";

    /// <summary>
    /// The number of the last starter slot.
    /// </summary>
    public const int LastStarterSlot = 11;

    /// <summary>
    /// The number of the first bench slot.
    /// </summary>
    public const int FirstBenchSlot = 12;

    /// <summary>
    /// The number of the last bench slot.
    /// </summary>
    public const int LastBenchSlot = 19;

    /// <summary>
    /// The rating assumed for a player who has none.
    /// </summary>
    public const int DefaultRating = 50;

    /// <summary>
    /// The fit, in percent, of a player in a slot whose position he has no rating for.
    /// </summary>
    public const int OffPositionFitPercent = 35;

    /// <summary>
    /// The fit, in percent, of a goalkeeper outside a goalkeeper slot, or of anyone in a goalkeeper slot without a goalkeeper rating.
    /// </summary>
    public const int GoalkeeperMismatchFitPercent = 5;

    /// <summary>
    /// The condition factor, in permille, of a player in the worst condition.
    /// </summary>
    public const int ConditionBasePermille = 700;

    /// <summary>
    /// The permille a full condition adds to <see cref="ConditionBasePermille"/>.
    /// </summary>
    public const int ConditionSpanPermille = 300;

    /// <summary>
    /// Picks the whole lineup for a formation: the eleven starters, then the bench.
    /// </summary>
    /// <param name="slots">The slots of the formation.</param>
    /// <param name="squad">The squad to pick from; suspended players and players without position data are skipped.</param>
    /// <returns>The picked players' identifiers by slot number; slots no eligible player is left for are absent.</returns>
    public IReadOnlyDictionary<int, int> Pick(IReadOnlyList<GetFormationSlotsResult> slots, IEnumerable<LineupCandidate> squad)
    {
        var eligible = squad
            .Where(player => !player.IsSuspended && player.HasPositionData)
            .OrderBy(player => player.PlayerId)
            .ToList();

        var starterSlots = slots
            .Where(slot => slot.SlotNumber is >= 1 and <= LastStarterSlot && slot.PositionId is not null)
            .OrderBy(slot => slot.SlotNumber)
            .ToList();

        var benchSlots = slots
            .Where(slot => slot.SlotNumber is >= FirstBenchSlot and <= LastBenchSlot)
            .OrderBy(slot => slot.SlotNumber)
            .ToList();

        var result = SeatStarters(starterSlots, eligible);

        var rest = eligible.Where(player => !result.ContainsValue(player.PlayerId)).ToList();

        // The first bench place goes to a reserve goalkeeper when there is one, since a lineup without one cannot cover an injury in goal.
        var isFirstBenchSlot = true;

        foreach (var benchSlot in benchSlots)
        {
            if (rest.Count == 0)
            {
                break;
            }

            var chosen = isFirstBenchSlot && rest.Any(player => player.Positions.Any(position => position.IsGoalkeeper))
                ? PickBest(rest.Where(player => player.Positions.Any(position => position.IsGoalkeeper)), BenchGoalkeeperScore)
                : PickBest(rest, BenchScore);

            result[benchSlot.SlotNumber] = chosen.PlayerId;
            rest.Remove(chosen);

            isFirstBenchSlot = false;
        }

        return result;
    }

    /// <summary>
    /// Seats players into open starter slots: the goalkeeper slot first, then the best remaining pairs.
    /// </summary>
    /// <param name="openSlots">The starter slots to fill.</param>
    /// <param name="players">The players to seat.</param>
    /// <returns>The seated players' identifiers by slot number; a slot or player left over is absent.</returns>
    public IReadOnlyDictionary<int, int> AssignStarters(IReadOnlyList<GetFormationSlotsResult> openSlots, IReadOnlyList<LineupCandidate> players)
    {
        return SeatStarters(openSlots, players);
    }

    private static SortedDictionary<int, int> SeatStarters(IReadOnlyList<GetFormationSlotsResult> openSlots, IReadOnlyList<LineupCandidate> players)
    {
        var result = new SortedDictionary<int, int>();
        var free = players.ToList();
        var slots = openSlots.OrderBy(slot => slot.SlotNumber).ToList();

        var goalkeeperSlot = slots.FirstOrDefault(IsGoalkeeperSlot);

        if (goalkeeperSlot is not null && free.Count > 0)
        {
            var keeper = PickBest(free, player => Score(player, goalkeeperSlot));

            result[goalkeeperSlot.SlotNumber] = keeper.PlayerId;
            free.Remove(keeper);
            slots.Remove(goalkeeperSlot);
        }

        var pairs = slots
            .SelectMany(slot => free.Select(player => (Slot: slot, Player: player, Score: Score(player, slot))))
            .OrderByDescending(pair => pair.Score)
            .ThenByDescending(pair => RatingOf(pair.Player))
            .ThenBy(pair => pair.Player.SquadNumber)
            .ThenBy(pair => pair.Player.PlayerId)
            .ThenBy(pair => pair.Slot.SlotNumber);

        var seatedPlayers = new HashSet<int>();

        foreach (var (slot, player, _) in pairs)
        {
            if (result.ContainsKey(slot.SlotNumber) || seatedPlayers.Contains(player.PlayerId))
            {
                continue;
            }

            result[slot.SlotNumber] = player.PlayerId;
            seatedPlayers.Add(player.PlayerId);
        }

        return result;
    }

    private static bool IsGoalkeeperSlot(GetFormationSlotsResult slot)
    {
        return slot.PositionArea == PositionArea.Goalkeeper;
    }

    private static int RatingOf(LineupCandidate player)
    {
        return player.Rating ?? DefaultRating;
    }

    private static int ConditionFactor(LineupCandidate player)
    {
        return ConditionBasePermille + ConditionSpanPermille * player.Condition / 100;
    }

    private static int FitPercent(LineupCandidate player, GetFormationSlotsResult slot)
    {
        var row = player.RowFor(slot.PositionId);

        if (row is not null)
        {
            return row.Quality;
        }

        return IsGoalkeeperSlot(slot) || player.OnlyGoalkeeper ? GoalkeeperMismatchFitPercent : OffPositionFitPercent;
    }

    private static long Score(LineupCandidate player, GetFormationSlotsResult slot)
    {
        return (long)RatingOf(player) * FitPercent(player, slot) * ConditionFactor(player);
    }

    private static long BenchScore(LineupCandidate player)
    {
        return (long)RatingOf(player) * ConditionFactor(player);
    }

    private static long BenchGoalkeeperScore(LineupCandidate player)
    {
        var bestGoalkeeperQuality = player.Positions.Where(position => position.IsGoalkeeper).Max(position => position.Quality);

        return (long)bestGoalkeeperQuality * RatingOf(player);
    }

    private static LineupCandidate PickBest(IEnumerable<LineupCandidate> players, Func<LineupCandidate, long> score)
    {
        return players
            .OrderByDescending(score)
            .ThenByDescending(RatingOf)
            .ThenBy(player => player.SquadNumber)
            .ThenBy(player => player.PlayerId)
            .First();
    }
}
