namespace SoccerManager.Application.Commands.Referee.CreateReferee;

/// <summary>
/// Maps <see cref="CreateRefereeRequest"/> instances to domain entities.
/// </summary>
public static class CreateRefereeMapper
{
    /// <summary>
    /// Creates a new referee entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the referee values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the referee.</param>
    /// <returns>A new, unsaved referee entity.</returns>
    public static SoccerManager.Domain.Entities.Referee ToDomainEntity(CreateRefereeRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.Referee
        {
            Name = request.Name,
            ImageUrl = request.ImageUrl,
            Tolerance = request.Tolerance,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
