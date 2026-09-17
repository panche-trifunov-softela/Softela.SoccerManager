namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a stadium for read-oriented consumers.
/// </summary>
public sealed record StadiumDto
{
    /// <summary>
    /// The identifier of the stadium.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the stadium.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The URL of the stadium's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// The stadium's capacity, as a number of spectators.
    /// </summary>
    public int Size { get; init; }

    /// <summary>
    /// The UTC date and time the stadium was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the stadium was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
