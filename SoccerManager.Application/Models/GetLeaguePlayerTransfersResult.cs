namespace SoccerManager.Application.Models;

/// <summary>
/// One deal row of the second result set returned by the <c>dbo.GetLeaguePlayerTransfers</c> stored procedure: an accepted
/// transfer or loan a player is part of in a league, denormalised with the club the player joined, the season and the player
/// the deal is about. Kept separate from <see cref="Dtos.PlayerTransferDto"/> because Dapper populates a result by setting
/// properties, and the Dto is an init-only record that cannot be a Dapper target.
/// </summary>
public class GetLeaguePlayerTransfersResult
{
    /// <summary>
    /// The identifier of the transfer or, for a loan, of the loan. The two tables count separately, so it is only unique
    /// together with <see cref="IsLoan"/>.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Whether the deal is a loan rather than a transfer.
    /// </summary>
    public bool IsLoan { get; set; }

    /// <summary>
    /// The identifier of the league team the player joined.
    /// </summary>
    public int LeagueTeamId { get; set; }

    /// <summary>
    /// The name of the club the player joined.
    /// </summary>
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// The URL of that club's logo, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; set; }

    /// <summary>
    /// The identifier of the season the deal was made in.
    /// </summary>
    public int SeasonId { get; set; }

    /// <summary>
    /// The number of that season within its league.
    /// </summary>
    public int SeasonNumber { get; set; }

    /// <summary>
    /// The transfer fee or, for a loan, the loan fee. 0 means no fee.
    /// </summary>
    public decimal Fee { get; set; }

    /// <summary>
    /// The identifier of the player the deal is about: the sold or the loaned player.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// The name of that player.
    /// </summary>
    public string PlayerName { get; set; } = string.Empty;
}
