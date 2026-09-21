namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a formation for read-oriented consumers.
/// </summary>
public sealed record FormationDto
{
    /// <summary>
    /// The identifier of the formation.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the formation.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The UTC date and time the formation was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the formation was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
