using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a transfer of a squad row between two clubs of a league for read-oriented consumers.
/// </summary>
public sealed record LeagueTeamPlayerTransferDto
{
    /// <summary>
    /// The identifier of the transfer.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the squad row (the player's registration at the selling club) that is transferred.
    /// </summary>
    public int LeagueTeamPlayerId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) that sells the player.
    /// </summary>
    public int FromLeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) that buys the player.
    /// </summary>
    public int ToLeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the season the transfer takes place in.
    /// </summary>
    public int SeasonId { get; init; }

    /// <summary>
    /// The fee the buying club pays, which is zero for a free transfer.
    /// </summary>
    public decimal TransferFee { get; init; }

    /// <summary>
    /// The price at which the selling club can buy the player back, or <see langword="null"/> when there is no buyback.
    /// </summary>
    public decimal? BuybackValue { get; init; }

    /// <summary>
    /// The number of seasons the buyback stays valid, or <see langword="null"/> when there is no buyback.
    /// </summary>
    public int? BuybackValidSeasons { get; init; }

    /// <summary>
    /// Where the transfer stands.
    /// </summary>
    public TransferStatus Status { get; init; }

    /// <summary>
    /// The UTC date and time the transfer was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the transfer was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
