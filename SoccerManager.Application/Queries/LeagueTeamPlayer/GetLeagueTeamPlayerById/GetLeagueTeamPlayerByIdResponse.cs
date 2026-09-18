using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayer.GetLeagueTeamPlayerById;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamPlayerByIdRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamPlayerByIdResponse
{
    /// <summary>
    /// The requested league team player.
    /// </summary>
    public required LeagueTeamPlayerDto Data { get; init; }
}
