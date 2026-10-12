using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamById;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamByIdRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamByIdResponse
{
    /// <summary>
    /// The requested league team.
    /// </summary>
    public required LeagueTeamDto Data { get; init; }
}
