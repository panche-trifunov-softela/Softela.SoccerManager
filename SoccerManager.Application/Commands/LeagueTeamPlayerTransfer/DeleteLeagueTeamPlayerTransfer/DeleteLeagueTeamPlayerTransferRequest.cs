using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.DeleteLeagueTeamPlayerTransfer;

/// <summary>
/// Represents a request to delete an existing transfer of a squad row.
/// </summary>
public sealed record DeleteLeagueTeamPlayerTransferRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the transfer to delete.
    /// </summary>
    public int Id { get; init; }
}
