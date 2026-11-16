using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.PlaceLeagueTeamPlayerTransferBid;

/// <summary>
/// Represents a request by a club to place a pending transfer bid for a player of another club of its league. The selling
/// club, the squad row and the season are not part of the request: the handler resolves them.
/// </summary>
public sealed record PlaceLeagueTeamPlayerTransferBidRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the league team (the club in its league) that bids. It is set from the route.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the player the club bids for. It is set from the route.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The fee the bidding club offers, which is zero for a free transfer.
    /// </summary>
    public decimal TransferFee { get; init; }

    /// <summary>
    /// The price at which the selling club could buy the player back, or <see langword="null"/> when the bid asks for no
    /// buyback. It is set together with <see cref="BuybackValidSeasons"/> or not at all.
    /// </summary>
    public decimal? BuybackValue { get; init; }

    /// <summary>
    /// The number of seasons the buyback would stay valid, 1 or 2, or <see langword="null"/> when the bid asks for no
    /// buyback. It is set together with <see cref="BuybackValue"/> or not at all.
    /// </summary>
    public int? BuybackValidSeasons { get; init; }

    /// <summary>
    /// The identifiers of the squad rows (the players at the bidding club) offered in the swap, at most 2. A request that
    /// leaves them out offers no swap.
    /// </summary>
    public IReadOnlyList<int> SwapLeagueTeamPlayerIds { get; init; } = [];
}
