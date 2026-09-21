using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchFormationPlayerPosition.GetMatchFormationPlayerPositionById;

/// <summary>
/// Maps match formation player position domain entities to <see cref="MatchFormationPlayerPositionDto"/> instances.
/// </summary>
public static class GetMatchFormationPlayerPositionByIdMapper
{
    /// <summary>
    /// Converts a match formation player position entity into its DTO representation.
    /// </summary>
    /// <param name="matchFormationPlayerPosition">The match formation player position entity to convert.</param>
    /// <returns>The corresponding <see cref="MatchFormationPlayerPositionDto"/>.</returns>
    public static MatchFormationPlayerPositionDto ToDto(SoccerManager.Domain.Entities.MatchFormationPlayerPosition matchFormationPlayerPosition)
    {
        return new MatchFormationPlayerPositionDto
        {
            Id = matchFormationPlayerPosition.Id,
            MatchId = matchFormationPlayerPosition.MatchId,
            TeamId = matchFormationPlayerPosition.TeamId,
            FormationPositionId = matchFormationPlayerPosition.FormationPositionId,
            PlayerPositionId = matchFormationPlayerPosition.PlayerPositionId,
            ConditionOnMatch = matchFormationPlayerPosition.ConditionOnMatch,
            QualityAtPositionOnMatch = matchFormationPlayerPosition.QualityAtPositionOnMatch,
            IsSuspended = matchFormationPlayerPosition.IsSuspended,
            CreatedAt = matchFormationPlayerPosition.CreatedAt,
            ModifiedAt = matchFormationPlayerPosition.ModifiedAt,
        };
    }
}
