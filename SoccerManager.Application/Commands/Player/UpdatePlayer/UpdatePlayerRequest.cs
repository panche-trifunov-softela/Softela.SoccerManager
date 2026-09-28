using MediatR;

namespace SoccerManager.Application.Commands.Player.UpdatePlayer;

/// <summary>
/// Represents a request to update an existing player.
/// </summary>
public sealed record UpdatePlayerRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the player to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The new name of the player.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The player's date of birth.
    /// </summary>
    public DateOnly DateOfBirth { get; init; }

    /// <summary>
    /// The player's overall rating.
    /// </summary>
    public int Rating { get; init; }

    /// <summary>
    /// The player's market value.
    /// </summary>
    public decimal Value { get; init; }

    /// <summary>
    /// The player's wage.
    /// </summary>
    public decimal Wage { get; init; }

    /// <summary>
    /// The URL of the player's image, if one has been set.
    /// </summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// The identifier of the national team the player plays for, if one has been assigned.
    /// </summary>
    public int? NationalTeamId { get; init; }
}
