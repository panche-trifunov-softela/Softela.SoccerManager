using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.AnswerLeagueTeamPlayerTransferBid;

/// <summary>
/// Represents a request by the club that holds a player to answer a pending transfer bid on him, by accepting or rejecting
/// it. An accepted bid only records the agreement: nobody moves.
/// </summary>
public sealed record AnswerLeagueTeamPlayerTransferBidRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the league team (the club in its league) that holds the player and answers the bid. It is set from
    /// the route.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the player the bid is for. It is set from the route.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The identifier of the transfer bid to answer. It is set from the route.
    /// </summary>
    public int BidId { get; init; }

    /// <summary>
    /// The answer to give: <see cref="TransferStatus.Accepted"/> or <see cref="TransferStatus.Rejected"/>. There is
    /// deliberately no default, so an omitted value is zero and fails validation.
    /// </summary>
    public TransferStatus Status { get; init; }
}
