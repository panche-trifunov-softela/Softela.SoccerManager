namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents one accepted transfer or loan a player is part of in a league: the club the player joined, the season, the fee,
/// the player the deal is about and the players swapped in it.
/// </summary>
public sealed record PlayerTransferDto
{
    /// <summary>
    /// The identifier of the transfer or, for a loan, of the loan. The two count separately, so it is only unique together
    /// with <see cref="IsLoan"/>.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Whether the deal is a loan rather than a transfer.
    /// </summary>
    public bool IsLoan { get; init; }

    /// <summary>
    /// The identifier of the league team the player joined.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The name of the club the player joined.
    /// </summary>
    public required string TeamName { get; init; }

    /// <summary>
    /// The URL of that club's logo, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; init; }

    /// <summary>
    /// The identifier of the season the deal was made in.
    /// </summary>
    public int SeasonId { get; init; }

    /// <summary>
    /// The number of that season within its league.
    /// </summary>
    public int SeasonNumber { get; init; }

    /// <summary>
    /// The transfer fee or, for a loan, the loan fee, in euros. 0 means no fee.
    /// </summary>
    public decimal Fee { get; init; }

    /// <summary>
    /// The identifier of the player the deal is about: the sold or the loaned player.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The name of that player.
    /// </summary>
    public required string PlayerName { get; init; }

    /// <summary>
    /// The players swapped in the transfer, in the order they were saved. Empty for a loan and for a transfer without a swap.
    /// </summary>
    public required IReadOnlyList<PlayerTransferSwapPlayerDto> SwapPlayers { get; init; }
}
