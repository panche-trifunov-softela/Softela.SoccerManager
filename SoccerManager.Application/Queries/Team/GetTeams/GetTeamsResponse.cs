using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Team.GetTeams;

/// <summary>
/// Represents the result of a <see cref="GetTeamsRequest"/> query.
/// </summary>
public sealed record GetTeamsResponse
{
    /// <summary>
    /// The list of all teams.
    /// </summary>
    public required List<TeamDto> Data { get; init; }
}
