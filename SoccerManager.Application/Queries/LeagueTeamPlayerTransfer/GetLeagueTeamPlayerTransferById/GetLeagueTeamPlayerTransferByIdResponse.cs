using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransfer.GetLeagueTeamPlayerTransferById;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamPlayerTransferByIdRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamPlayerTransferByIdResponse
{
    /// <summary>
    /// The requested transfer.
    /// </summary>
    public required LeagueTeamPlayerTransferDto Data { get; init; }
}
