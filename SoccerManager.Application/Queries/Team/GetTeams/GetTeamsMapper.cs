using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Team.GetTeams;

/// <summary>
/// Maps team domain entities to <see cref="TeamDto"/> instances.
/// </summary>
public static class GetTeamsMapper
{
    /// <summary>
    /// Converts a team entity into its DTO representation.
    /// </summary>
    /// <param name="team">The team entity to convert.</param>
    /// <returns>The corresponding <see cref="TeamDto"/>.</returns>
    public static TeamDto ToDto(SoccerManager.Domain.Entities.Team team)
    {
        return new TeamDto
        {
            Id = team.Id,
            Name = team.Name,
            StadiumId = team.StadiumId,
            FinancialState = team.FinancialState,
            JerseyUrl = team.JerseyUrl,
            LogoUrl = team.LogoUrl,
            TransfermarktId = team.TransfermarktId,
            CreatedAt = team.CreatedAt,
            ModifiedAt = team.ModifiedAt,
        };
    }
}
