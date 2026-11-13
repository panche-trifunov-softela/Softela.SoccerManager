using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransferSwap.DeleteLeagueTeamPlayerTransferSwap;

/// <summary>
/// Represents a request to delete an existing swap.
/// </summary>
public sealed record DeleteLeagueTeamPlayerTransferSwapRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the swap to delete.
    /// </summary>
    public int Id { get; init; }
}
