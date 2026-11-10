using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.PenaltyAfterExtraTime.GetPenaltyAfterExtraTimeById;

/// <summary>
/// Maps penalty after extra time domain entities to <see cref="PenaltyAfterExtraTimeDto"/> instances.
/// </summary>
public static class GetPenaltyAfterExtraTimeByIdMapper
{
    /// <summary>
    /// Converts a penalty after extra time entity into its DTO representation.
    /// </summary>
    /// <param name="penaltyAfterExtraTime">The penalty after extra time entity to convert.</param>
    /// <returns>The corresponding <see cref="PenaltyAfterExtraTimeDto"/>.</returns>
    public static PenaltyAfterExtraTimeDto ToDto(SoccerManager.Domain.Entities.PenaltyAfterExtraTime penaltyAfterExtraTime)
    {
        return new PenaltyAfterExtraTimeDto
        {
            Id = penaltyAfterExtraTime.Id,
            MatchId = penaltyAfterExtraTime.MatchId,
            KickNumber = penaltyAfterExtraTime.KickNumber,
            LeagueTeamId = penaltyAfterExtraTime.LeagueTeamId,
            PlayerId = penaltyAfterExtraTime.PlayerId,
            IsScored = penaltyAfterExtraTime.IsScored,
            CreatedAt = penaltyAfterExtraTime.CreatedAt,
            ModifiedAt = penaltyAfterExtraTime.ModifiedAt,
        };
    }
}
