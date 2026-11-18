using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerBids;

/// <summary>
/// Represents a request to retrieve the pending bids on a player of a club.
/// </summary>
public sealed record GetLeaguePlayerBidsRequest : IRequest<GetLeaguePlayerBidsResponse>
{
    /// <summary>
    /// The identifier of the league team that holds the player, which the caller is managing.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the player whose pending bids are being requested.
    /// </summary>
    public int PlayerId { get; init; }
}
