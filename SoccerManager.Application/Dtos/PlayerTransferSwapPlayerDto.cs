namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a player swapped in a transfer: a player of the buying club who went to the selling club as part of the deal.
/// </summary>
public sealed record PlayerTransferSwapPlayerDto
{
    /// <summary>
    /// The identifier of the swapped player.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// The name of the swapped player.
    /// </summary>
    public required string Name { get; init; }
}
