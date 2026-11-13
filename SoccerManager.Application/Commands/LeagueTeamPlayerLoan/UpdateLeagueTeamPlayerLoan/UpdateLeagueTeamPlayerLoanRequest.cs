using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerLoan.UpdateLeagueTeamPlayerLoan;

/// <summary>
/// Represents a request to update an existing loan.
/// </summary>
public sealed record UpdateLeagueTeamPlayerLoanRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the loan to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the squad row (<c>dbo.LeagueTeamPlayers</c>) of the loaned player at the lending club.
    /// </summary>
    public int LeagueTeamPlayerId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) the player is loaned to.
    /// </summary>
    public int ToLeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the season the loan starts in.
    /// </summary>
    public int SeasonId { get; init; }

    /// <summary>
    /// The fee the loan club pays for the loan. Zero means the loan is free.
    /// </summary>
    public decimal LoanFee { get; init; }

    /// <summary>
    /// The number of seasons the loan lasts, 1 or 2.
    /// </summary>
    public int SeasonsLength { get; init; }

    /// <summary>
    /// The price at which the loan club may buy the player, or <see langword="null"/> when there is no option to buy.
    /// </summary>
    public decimal? BuyOptionValue { get; init; }

    /// <summary>
    /// The share of the player's wage the loan club pays, as a whole percentage from 0 to 100.
    /// </summary>
    public int SalaryCoverageInPercent { get; init; }

    /// <summary>
    /// Where the loan stands.
    /// </summary>
    public TransferStatus Status { get; init; }
}
