namespace SoccerManager.Application.Commands.Standing.CreateStanding;

/// <summary>
/// Maps <see cref="CreateStandingRequest"/> instances to domain entities.
/// </summary>
public static class CreateStandingMapper
{
    /// <summary>
    /// Creates a new standing entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the standing values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the standing.</param>
    /// <returns>A new, unsaved standing entity.</returns>
    public static SoccerManager.Domain.Entities.Standing ToDomainEntity(CreateStandingRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.Standing
        {
            SeasonId = request.SeasonId,
            DivisionId = request.DivisionId,
            TeamId = request.TeamId,
            Points = request.Points,
            GoalsFor = request.GoalsFor,
            GoalsAgainst = request.GoalsAgainst,
            Wins = request.Wins,
            Draws = request.Draws,
            Losses = request.Losses,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
