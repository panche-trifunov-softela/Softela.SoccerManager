namespace SoccerManager.Application.Models;

/// <summary>
/// One row of the swapped-player result set returned by the <c>dbo.GetLeaguePlayerBids</c> stored procedure (its third) and
/// by the <c>dbo.GetLeagueTeamTransferBids</c> stored procedure (its second): a player the bidding club offers in a transfer
/// bid the procedure lists, with the main position and the current rating. Kept separate from
/// <see cref="Dtos.LeaguePlayerBidSwapPlayerDto"/> because Dapper populates a result by setting properties, and the Dto is an
/// init-only record that cannot be a Dapper target.
/// </summary>
public class GetLeaguePlayerTransferBidSwapPlayersResult
{
    /// <summary>
    /// The identifier of the transfer bid the player is offered in.
    /// </summary>
    public int LeagueTeamPlayerTransferId { get; set; }

    /// <summary>
    /// The identifier of the offered player.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// The name of the offered player.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The short name of the player's main position, for example CB.
    /// </summary>
    public string PositionShortName { get; set; } = string.Empty;

    /// <summary>
    /// The player's current rating, or <see langword="null"/> when it has none.
    /// </summary>
    public int? Rating { get; set; }
}
