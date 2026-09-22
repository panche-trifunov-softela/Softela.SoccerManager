using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Match.GetMatchById;

/// <summary>
/// Maps match domain entities to <see cref="MatchDto"/> instances.
/// </summary>
public static class GetMatchByIdMapper
{
    /// <summary>
    /// Converts a match entity into its DTO representation.
    /// </summary>
    /// <param name="match">The match entity to convert.</param>
    /// <returns>The corresponding <see cref="MatchDto"/>.</returns>
    public static MatchDto ToDto(SoccerManager.Domain.Entities.Match match)
    {
        return new MatchDto
        {
            Id = match.Id,
            SeasonId = match.SeasonId,
            CompetitionId = match.CompetitionId,
            RefereeId = match.RefereeId,
            StartDateTime = match.StartDateTime,
            Commentary = match.Commentary,
            Attendance = match.Attendance,
            IsStarted = match.IsStarted,
            CreatedAt = match.CreatedAt,
            ModifiedAt = match.ModifiedAt,
        };
    }
}
