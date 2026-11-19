using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamLoanBids;

/// <summary>
/// Represents a request to retrieve the loan bids a club placed and received.
/// </summary>
public sealed record GetLeagueTeamLoanBidsRequest : IRequest<GetLeagueTeamLoanBidsResponse>
{
    /// <summary>
    /// The identifier of the league team whose loan bids are being requested, which the caller is managing.
    /// </summary>
    public int LeagueTeamId { get; init; }
}
