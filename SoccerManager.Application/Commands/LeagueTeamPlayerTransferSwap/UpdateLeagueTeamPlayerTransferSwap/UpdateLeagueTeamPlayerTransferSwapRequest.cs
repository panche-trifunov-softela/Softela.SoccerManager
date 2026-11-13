using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransferSwap.UpdateLeagueTeamPlayerTransferSwap;

/// <summary>
/// Represents a request to update an existing swap.
/// </summary>
public sealed record UpdateLeagueTeamPlayerTransferSwapRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the swap to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the transfer the swapped player is part of.
    /// </summary>
    public int LeagueTeamPlayerTransferId { get; init; }

    /// <summary>
    /// The identifier of the squad row (the swapped player's registration at the buying club) of the player who goes to the selling club.
    /// </summary>
    public int LeagueTeamPlayerId { get; init; }
}
