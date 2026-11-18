using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerBidders;

/// <summary>
/// Represents the result of a <see cref="GetLeaguePlayerBiddersRequest"/> query.
/// </summary>
public sealed record GetLeaguePlayerBiddersResponse
{
    /// <summary>
    /// The clubs that have bid for the requested player in the league, accepted deals first, then newest first.
    /// </summary>
    public required IReadOnlyList<LeaguePlayerBidderDto> Data { get; init; }
}
