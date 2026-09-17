using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Stadium.GetStadiums;

/// <summary>
/// Maps stadium domain entities to <see cref="StadiumDto"/> instances.
/// </summary>
public static class GetStadiumsMapper
{
    /// <summary>
    /// Converts a stadium entity into its DTO representation.
    /// </summary>
    /// <param name="stadium">The stadium entity to convert.</param>
    /// <returns>The corresponding <see cref="StadiumDto"/>.</returns>
    public static StadiumDto ToDto(SoccerManager.Domain.Entities.Stadium stadium)
    {
        return new StadiumDto
        {
            Id = stadium.Id,
            Name = stadium.Name,
            ImageUrl = stadium.ImageUrl,
            Size = stadium.Size,
            CreatedAt = stadium.CreatedAt,
            ModifiedAt = stadium.ModifiedAt,
        };
    }
}
