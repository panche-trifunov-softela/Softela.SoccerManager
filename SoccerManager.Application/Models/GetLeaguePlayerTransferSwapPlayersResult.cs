namespace SoccerManager.Application.Models;

/// <summary>
/// One row of the third result set returned by the <c>dbo.GetLeaguePlayerTransfers</c> stored procedure: a player swapped in
/// an accepted transfer. Kept separate from <see cref="Dtos.PlayerTransferSwapPlayerDto"/> because Dapper populates a
/// result by setting properties, and the Dto is an init-only record that cannot be a Dapper target.
/// </summary>
public class GetLeaguePlayerTransferSwapPlayersResult
{
    /// <summary>
    /// The identifier of the transfer the player was swapped in.
    /// </summary>
    public int LeagueTeamPlayerTransferId { get; set; }

    /// <summary>
    /// The identifier of the swapped player.
    /// </summary>
    public int PlayerId { get; set; }

    /// <summary>
    /// The name of the swapped player.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}
