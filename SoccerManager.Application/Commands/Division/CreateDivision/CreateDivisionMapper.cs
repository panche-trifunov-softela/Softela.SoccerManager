namespace SoccerManager.Application.Commands.Division.CreateDivision;

/// <summary>
/// Maps <see cref="CreateDivisionRequest"/> instances to domain entities.
/// </summary>
public static class CreateDivisionMapper
{
    /// <summary>
    /// Creates a new division entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the league identifier and division values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the division.</param>
    /// <returns>A new, unsaved division entity.</returns>
    public static SoccerManager.Domain.Entities.Division ToDomainEntity(CreateDivisionRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.Division
        {
            LeagueId = request.LeagueId,
            Name = request.Name,
            Order = request.Order,
            TeamsPromoted = request.TeamsPromoted,
            TeamsRelegated = request.TeamsRelegated,
            TeamsInPlayoffs = request.TeamsInPlayoffs,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
