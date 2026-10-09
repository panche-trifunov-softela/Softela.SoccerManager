using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetLeagueTeamManagerApplicationById;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamManagerApplicationByIdRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamManagerApplicationByIdResponse
{
    /// <summary>
    /// The requested league team manager application.
    /// </summary>
    public required LeagueTeamManagerApplicationDto Data { get; init; }
}
