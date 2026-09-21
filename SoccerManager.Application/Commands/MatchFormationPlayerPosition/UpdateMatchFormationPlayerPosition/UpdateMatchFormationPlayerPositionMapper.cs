using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.MatchFormationPlayerPosition.UpdateMatchFormationPlayerPosition;

/// <summary>
/// Applies <see cref="UpdateMatchFormationPlayerPositionRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateMatchFormationPlayerPositionMapper
{
    /// <summary>
    /// Applies the request and snapshot values to the given match formation player position, preserving
    /// its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new match formation player position ids.</param>
    /// <param name="snapshot">The values snapshotted from the player's league registration and position rating.</param>
    /// <param name="matchFormationPlayerPosition">The loaded match formation player position entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateMatchFormationPlayerPositionRequest request, MatchFormationPlayerPositionSnapshot snapshot, SoccerManager.Domain.Entities.MatchFormationPlayerPosition matchFormationPlayerPosition, DateTime now, Guid userId)
    {
        matchFormationPlayerPosition.MatchId = request.MatchId;
        matchFormationPlayerPosition.TeamId = request.TeamId;
        matchFormationPlayerPosition.FormationPositionId = request.FormationPositionId;
        matchFormationPlayerPosition.PlayerPositionId = request.PlayerPositionId;
        matchFormationPlayerPosition.ConditionOnMatch = snapshot.ConditionOnMatch;
        matchFormationPlayerPosition.QualityAtPositionOnMatch = snapshot.QualityAtPositionOnMatch;
        matchFormationPlayerPosition.IsSuspended = snapshot.IsSuspended;
        matchFormationPlayerPosition.ModifiedAt = now;
        matchFormationPlayerPosition.ModifiedBy = userId;
    }
}
