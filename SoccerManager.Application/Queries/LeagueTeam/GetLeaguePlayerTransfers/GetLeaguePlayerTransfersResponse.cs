using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerTransfers;

/// <summary>
/// Represents the result of a <see cref="GetLeaguePlayerTransfersRequest"/> query.
/// </summary>
public sealed record GetLeaguePlayerTransfersResponse
{
    /// <summary>
    /// The requested player's accepted transfers and loans in the league, newest season first.
    /// </summary>
    public required List<PlayerTransferDto> Data { get; init; }
}
