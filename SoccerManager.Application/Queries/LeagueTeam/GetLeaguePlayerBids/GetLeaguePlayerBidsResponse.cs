using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerBids;

/// <summary>
/// Represents the result of a <see cref="GetLeaguePlayerBidsRequest"/> query.
/// </summary>
public sealed record GetLeaguePlayerBidsResponse
{
    /// <summary>
    /// The pending transfer and loan bids on the requested player, each newest first.
    /// </summary>
    public required LeaguePlayerBidsDto Data { get; init; }
}
