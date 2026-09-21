using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.MatchFormationPlayerPosition.CreateMatchFormationPlayerPosition;

/// <summary>
/// Maps <see cref="CreateMatchFormationPlayerPositionRequest"/> instances to domain entities.
/// </summary>
public static class CreateMatchFormationPlayerPositionMapper
{
    /// <summary>
    /// Creates a new match formation player position entity from the given request and snapshot.
    /// </summary>
    /// <param name="request">The request carrying the match formation player position ids.</param>
    /// <param name="snapshot">The values snapshotted from the player's league registration and position rating.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the match formation player position.</param>
    /// <returns>A new, unsaved match formation player position entity.</returns>
    public static SoccerManager.Domain.Entities.MatchFormationPlayerPosition ToDomainEntity(CreateMatchFormationPlayerPositionRequest request, MatchFormationPlayerPositionSnapshot snapshot, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.MatchFormationPlayerPosition
        {
            MatchId = request.MatchId,
            TeamId = request.TeamId,
            FormationPositionId = request.FormationPositionId,
            PlayerPositionId = request.PlayerPositionId,
            ConditionOnMatch = snapshot.ConditionOnMatch,
            QualityAtPositionOnMatch = snapshot.QualityAtPositionOnMatch,
            IsSuspended = snapshot.IsSuspended,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
