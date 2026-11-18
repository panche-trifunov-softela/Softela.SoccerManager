using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerBidders;

/// <summary>
/// Represents a request to retrieve the clubs that have bid for a player in the league of a league team.
/// </summary>
public sealed record GetLeaguePlayerBiddersRequest : IRequest<GetLeaguePlayerBiddersResponse>
{
    /// <summary>
    /// The identifier of the league team the caller is managing, whose league is the one to look in.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the player whose bidders are being requested.
    /// </summary>
    public int PlayerId { get; init; }
}
