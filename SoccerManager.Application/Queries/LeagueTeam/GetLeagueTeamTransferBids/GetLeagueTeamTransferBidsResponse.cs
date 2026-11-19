using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamTransferBids;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamTransferBidsRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamTransferBidsResponse
{
    /// <summary>
    /// The club's transfer bids, placed and received, split into pending and completed, each newest first.
    /// </summary>
    public required LeagueTeamTransferBidsDto Data { get; init; }
}
