using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamManager.GetLeagueTeamManagerById;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamManagerByIdRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamManagerByIdResponse
{
    /// <summary>
    /// The requested league team manager appointment.
    /// </summary>
    public required LeagueTeamManagerDto Data { get; init; }
}
