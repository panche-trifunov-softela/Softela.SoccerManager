using SoccerManager.Domain.Enums;

namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents the transfer of one squad row (one player's registration at one club of a league) to another club of the
/// same league in a season.
/// </summary>
public class LeagueTeamPlayerTransfer : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the squad row (the player's registration at the selling club) that is transferred.
    /// </summary>
    public int LeagueTeamPlayerId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the league team (the club in its league) that sells the player. It is saved from the
    /// squad row when the transfer is saved, so it survives the player's later move to another club.
    /// </summary>
    public int FromLeagueTeamId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the league team (the club in its league) that buys the player.
    /// </summary>
    public int ToLeagueTeamId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the season the transfer takes place in.
    /// </summary>
    public int SeasonId { get; set; }

    /// <summary>
    /// Gets or sets the fee the buying club pays, which is zero for a free transfer.
    /// </summary>
    public decimal TransferFee { get; set; }

    /// <summary>
    /// Gets or sets the price at which the selling club can buy the player back, or <see langword="null"/> when there is no
    /// buyback. It is set together with <see cref="BuybackValidSeasons"/> or not at all.
    /// </summary>
    public decimal? BuybackValue { get; set; }

    /// <summary>
    /// Gets or sets the number of seasons the buyback stays valid, or <see langword="null"/> when there is no buyback. It is
    /// set together with <see cref="BuybackValue"/> or not at all.
    /// </summary>
    public int? BuybackValidSeasons { get; set; }

    /// <summary>
    /// Gets or sets where the transfer stands.
    /// </summary>
    public TransferStatus Status { get; set; }
}
