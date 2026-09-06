namespace SoccerManager.Application.Commands.League.CreateLeague;

/// <summary>
/// Maps <see cref="CreateLeagueRequest"/> instances to domain entities.
/// </summary>
public static class CreateLeagueMapper
{
    /// <summary>
    /// Creates a new league entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the league name.</param>
    /// <param name="now">The timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the league.</param>
    /// <returns>A new, unsaved league entity.</returns>
    public static SoccerManager.Domain.Entities.League ToDomainEntity(CreateLeagueRequest request, DateTimeOffset now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.League
        {
            Name = request.Name,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
