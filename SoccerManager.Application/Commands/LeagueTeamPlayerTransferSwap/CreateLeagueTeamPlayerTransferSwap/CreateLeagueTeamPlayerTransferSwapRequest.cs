using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransferSwap.CreateLeagueTeamPlayerTransferSwap;

/// <summary>
/// Represents a request to add a swapped player to a transfer.
/// </summary>
public sealed record CreateLeagueTeamPlayerTransferSwapRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the transfer the swapped player is part of.
    /// </summary>
    public int LeagueTeamPlayerTransferId { get; init; }

    /// <summary>
    /// The identifier of the squad row (the swapped player's registration at the buying club) of the player who goes to the selling club.
    /// </summary>
    public int LeagueTeamPlayerId { get; init; }
}
