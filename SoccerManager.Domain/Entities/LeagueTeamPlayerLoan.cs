using SoccerManager.Domain.Enums;

namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents the loan of a squad row (one player's registration at one club of one league) to another club of the
/// same league for one or two seasons.
/// </summary>
public class LeagueTeamPlayerLoan : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the squad row (<c>dbo.LeagueTeamPlayers</c>) of the loaned player at the lending club.
    /// </summary>
    public int LeagueTeamPlayerId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the league team (the club in its league) the player is loaned to.
    /// </summary>
    public int ToLeagueTeamId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the season the loan starts in.
    /// </summary>
    public int SeasonId { get; set; }

    /// <summary>
    /// Gets or sets the fee the loan club pays for the loan. Zero means the loan is free.
    /// </summary>
    public decimal LoanFee { get; set; }

    /// <summary>
    /// Gets or sets the number of seasons the loan lasts, 1 or 2.
    /// </summary>
    public int SeasonsLength { get; set; }

    /// <summary>
    /// Gets or sets the price at which the loan club may buy the player, or <see langword="null"/> when there is no
    /// option to buy.
    /// </summary>
    public decimal? BuyOptionValue { get; set; }

    /// <summary>
    /// Gets or sets the share of the player's wage the loan club pays, as a whole percentage from 0 to 100.
    /// </summary>
    public int SalaryCoverageInPercent { get; set; }

    /// <summary>
    /// Gets or sets where the loan stands.
    /// </summary>
    public TransferStatus Status { get; set; }
}
