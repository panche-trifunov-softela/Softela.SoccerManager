namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a player the bidding club offers in a transfer bid: a player of the bidding club who would go to the selling
/// club as part of the deal.
/// </summary>
public sealed record LeaguePlayerBidSwapPlayerDto
{
    /// <summary>
    /// The identifier of the offered player.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The name of the offered player.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The short name of the player's main position, for example CB.
    /// </summary>
    public required string PositionShortName { get; init; }

    /// <summary>
    /// The player's current rating, or <see langword="null"/> when it has none.
    /// </summary>
    public int? Rating { get; init; }
}
