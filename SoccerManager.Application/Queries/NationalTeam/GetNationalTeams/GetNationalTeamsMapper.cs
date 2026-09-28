using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.NationalTeam.GetNationalTeams;

/// <summary>
/// Maps national team domain entities to <see cref="NationalTeamDto"/> instances.
/// </summary>
public static class GetNationalTeamsMapper
{
    /// <summary>
    /// Converts a national team entity into its DTO representation.
    /// </summary>
    /// <param name="nationalTeam">The national team entity to convert.</param>
    /// <returns>The corresponding <see cref="NationalTeamDto"/>.</returns>
    public static NationalTeamDto ToDto(SoccerManager.Domain.Entities.NationalTeam nationalTeam)
    {
        return new NationalTeamDto
        {
            Id = nationalTeam.Id,
            Name = nationalTeam.Name,
            StadiumId = nationalTeam.StadiumId,
            JerseyUrl = nationalTeam.JerseyUrl,
            LogoUrl = nationalTeam.LogoUrl,
            CreatedAt = nationalTeam.CreatedAt,
            ModifiedAt = nationalTeam.ModifiedAt,
        };
    }
}
