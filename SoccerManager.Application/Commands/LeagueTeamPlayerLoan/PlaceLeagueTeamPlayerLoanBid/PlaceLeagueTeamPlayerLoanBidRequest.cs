using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerLoan.PlaceLeagueTeamPlayerLoanBid;

/// <summary>
/// Represents a request by a club to place a pending loan bid for a player of another club of its league. The lending
/// club, the squad row and the season are not part of the request: the handler resolves them.
/// </summary>
public sealed record PlaceLeagueTeamPlayerLoanBidRequest : IRequest<int>
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
    /// The fee the bidding club offers for the loan. Zero means the loan is free.
    /// </summary>
    public decimal LoanFee { get; init; }

    /// <summary>
    /// The number of seasons the loan would last, 1 or 2.
    /// </summary>
    public int SeasonsLength { get; init; }

    /// <summary>
    /// The price at which the bidding club could buy the player, or <see langword="null"/> when the bid asks for no option
    /// to buy.
    /// </summary>
    public decimal? BuyOptionValue { get; init; }
}
