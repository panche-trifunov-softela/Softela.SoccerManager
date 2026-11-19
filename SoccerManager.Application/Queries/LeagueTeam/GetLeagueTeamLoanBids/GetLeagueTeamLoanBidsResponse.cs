using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamLoanBids;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamLoanBidsRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamLoanBidsResponse
{
    /// <summary>
    /// The club's loan bids, placed and received, split into pending and completed, each newest first.
    /// </summary>
    public required LeagueTeamLoanBidsDto Data { get; init; }
}
