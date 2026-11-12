using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetGameWorldPlayerProfile;

/// <summary>
/// Represents a request to retrieve a player's profile in the game world of a league team's league.
/// </summary>
public sealed record GetGameWorldPlayerProfileRequest : IRequest<GetGameWorldPlayerProfileResponse>
{
    /// <summary>
    /// The identifier of the league team the caller is managing, whose league is the game world to look in.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the player whose profile is being requested.
    /// </summary>
    public int PlayerId { get; init; }
}
