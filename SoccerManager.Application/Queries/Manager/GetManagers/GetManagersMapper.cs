using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Manager.GetManagers;

/// <summary>
/// Maps manager domain entities to <see cref="ManagerDto"/> instances.
/// </summary>
public static class GetManagersMapper
{
    /// <summary>
    /// Converts a manager entity into its DTO representation.
    /// </summary>
    /// <param name="manager">The manager entity to convert.</param>
    /// <returns>The corresponding <see cref="ManagerDto"/>.</returns>
    public static ManagerDto ToDto(SoccerManager.Domain.Entities.Manager manager)
    {
        return new ManagerDto
        {
            Id = manager.Id,
            UserId = manager.UserId,
            ImageUrl = manager.ImageUrl,
            CreatedAt = manager.CreatedAt,
            ModifiedAt = manager.ModifiedAt,
        };
    }
}
