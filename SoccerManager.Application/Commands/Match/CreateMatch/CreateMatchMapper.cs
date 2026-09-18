namespace SoccerManager.Application.Commands.Match.CreateMatch;

/// <summary>
/// Maps <see cref="CreateMatchRequest"/> instances to domain entities.
/// </summary>
public static class CreateMatchMapper
{
    /// <summary>
    /// Creates a new match entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the match values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the match.</param>
    /// <returns>A new, unsaved match entity.</returns>
    public static SoccerManager.Domain.Entities.Match ToDomainEntity(CreateMatchRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.Match
        {
            SeasonId = request.SeasonId,
            DivisionId = request.DivisionId,
            RefereeId = request.RefereeId,
            StartDateTime = request.StartDateTime,
            Commentary = request.Commentary,
            Attendance = request.Attendance,
            IsStarted = request.IsStarted,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
