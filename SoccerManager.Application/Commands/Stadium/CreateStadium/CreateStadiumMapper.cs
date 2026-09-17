namespace SoccerManager.Application.Commands.Stadium.CreateStadium;

/// <summary>
/// Maps <see cref="CreateStadiumRequest"/> instances to domain entities.
/// </summary>
public static class CreateStadiumMapper
{
    /// <summary>
    /// Creates a new stadium entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the stadium values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the stadium.</param>
    /// <returns>A new, unsaved stadium entity.</returns>
    public static SoccerManager.Domain.Entities.Stadium ToDomainEntity(CreateStadiumRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.Stadium
        {
            Name = request.Name,
            ImageUrl = request.ImageUrl,
            Size = request.Size,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
