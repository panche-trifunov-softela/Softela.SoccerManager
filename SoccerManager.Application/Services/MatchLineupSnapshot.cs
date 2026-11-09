using SoccerManager.Application.Core.Extensions;
using SoccerManager.Application.Models;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Services;

/// <summary>
/// Builds the lineup row that seats a squad player in a formation slot, snapshotting the player's condition, quality for the
/// slot and suspension, so a main lineup and a substitution lineup snapshot a player the same way.
/// </summary>
public static class MatchLineupSnapshot
{
    /// <summary>
    /// Creates the unsaved row that seats a player in a slot. The row points at the player's rating for the slot's position,
    /// else their main one; only a starter slot they have no rating for gets a quality worked out from the two areas.
    /// </summary>
    /// <typeparam name="TRow">The lineup row type to create.</typeparam>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) the row is for.</param>
    /// <param name="slot">The formation slot the player fills.</param>
    /// <param name="candidate">The player, from the club's squad; they must have position data.</param>
    /// <param name="now">The UTC moment stamped on the row.</param>
    /// <param name="userId">The identifier of the user stamped on the row.</param>
    /// <returns>A new, unsaved row.</returns>
    public static TRow ToRow<TRow>(int matchId, int leagueTeamId, GetFormationSlotsResult slot, LineupCandidate candidate, DateTime now, Guid userId)
        where TRow : MatchFormationPlayerPosition, new()
    {
        // The row always points at a rating the player has: the one for the slot's position, else their main one. Only a
        // starter slot they have no rating for gets a quality worked out from the two areas rather than copied.
        var rating = candidate.RowFor(slot.PositionId);
        var main = candidate.Main!;
        var quality = rating?.Quality ?? slot.PositionArea?.ToUnratedQuality(main.Area) ?? main.Quality;

        return new TRow
        {
            MatchId = matchId,
            LeagueTeamId = leagueTeamId,
            FormationPositionId = slot.FormationPositionId,
            PlayerPositionId = (rating ?? main).PlayerPositionId,
            ConditionOnMatch = candidate.Condition,
            QualityAtPositionOnMatch = quality,
            IsSuspended = candidate.IsSuspended,
            CreatedAt = now,
            ModifiedAt = now,
            CreatedBy = userId,
            ModifiedBy = userId,
        };
    }
}
