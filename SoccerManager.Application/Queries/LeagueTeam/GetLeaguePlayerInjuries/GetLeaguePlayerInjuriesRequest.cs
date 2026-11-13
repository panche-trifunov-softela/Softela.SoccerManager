using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerInjuries;

/// <summary>
/// Represents a request to retrieve a player's injuries in the league of a league team.
/// </summary>
public sealed record GetLeaguePlayerInjuriesRequest : IRequest<GetLeaguePlayerInjuriesResponse>
{
    /// <summary>
    /// The identifier of the league team the caller is managing, whose league is the one to look in.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the player whose injuries are being requested.
    /// </summary>
    public int PlayerId { get; init; }
}
