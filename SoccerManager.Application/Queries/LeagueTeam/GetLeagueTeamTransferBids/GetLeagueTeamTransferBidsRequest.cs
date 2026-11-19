using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamTransferBids;

/// <summary>
/// Represents a request to retrieve the transfer bids a club placed and received.
/// </summary>
public sealed record GetLeagueTeamTransferBidsRequest : IRequest<GetLeagueTeamTransferBidsResponse>
{
    /// <summary>
    /// The identifier of the league team whose transfer bids are being requested, which the caller is managing.
    /// </summary>
    public int LeagueTeamId { get; init; }
}
