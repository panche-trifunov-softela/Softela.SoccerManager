using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Team.GetTeamById;

/// <summary>
/// Represents the result of a <see cref="GetTeamByIdRequest"/> query.
/// </summary>
public sealed record GetTeamByIdResponse
{
    /// <summary>
    /// The requested team.
    /// </summary>
    public required TeamDto Data { get; init; }
}
