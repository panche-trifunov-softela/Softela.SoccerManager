using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerTransferSwap.GetLeagueTeamPlayerTransferSwaps;

/// <summary>
/// Represents a request to retrieve every swapped player of a transfer.
/// </summary>
public sealed record GetLeagueTeamPlayerTransferSwapsRequest : IRequest<GetLeagueTeamPlayerTransferSwapsResponse>
{
    /// <summary>
    /// The identifier of the transfer whose swaps are being requested.
    /// </summary>
    public int LeagueTeamPlayerTransferId { get; init; }
}
