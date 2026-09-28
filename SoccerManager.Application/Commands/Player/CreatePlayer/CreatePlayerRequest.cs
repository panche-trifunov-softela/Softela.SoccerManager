using MediatR;

namespace SoccerManager.Application.Commands.Player.CreatePlayer;

/// <summary>
/// Represents a request to create a new player.
/// </summary>
public sealed record CreatePlayerRequest : IRequest<int>
{
    /// <summary>
    /// The name of the player to create.
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
