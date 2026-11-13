using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerProfile;

/// <summary>
/// Represents a request to retrieve a player's profile in the league of a league team.
/// </summary>
public sealed record GetLeaguePlayerProfileRequest : IRequest<GetLeaguePlayerProfileResponse>
{
    /// <summary>
    /// The identifier of the league team the caller is managing, whose league is the one to look in.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the player whose profile is being requested.
    /// </summary>
    public int PlayerId { get; init; }
}
