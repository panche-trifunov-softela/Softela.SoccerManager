namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a swapped player of a transfer for read-oriented consumers.
/// </summary>
public sealed record LeagueTeamPlayerTransferSwapDto
{
    /// <summary>
    /// The identifier of the swap.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the transfer the swapped player is part of.
    /// </summary>
    public int LeagueTeamPlayerTransferId { get; init; }

    /// <summary>
    /// The identifier of the squad row (the swapped player's registration at the buying club) of the player who goes to the selling club.
    /// </summary>
    public int LeagueTeamPlayerId { get; init; }

    /// <summary>
    /// The UTC date and time the swap was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the swap was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
