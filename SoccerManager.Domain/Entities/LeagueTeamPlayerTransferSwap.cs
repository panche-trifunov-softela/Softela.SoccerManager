namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a player of the buying club who goes to the selling club as part of a transfer.
/// </summary>
public class LeagueTeamPlayerTransferSwap : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the transfer the swapped player is part of.
    /// </summary>
    public int LeagueTeamPlayerTransferId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the squad row (the swapped player's registration at the buying club) of the player who goes to the selling club.
    /// </summary>
    public int LeagueTeamPlayerId { get; set; }
}
