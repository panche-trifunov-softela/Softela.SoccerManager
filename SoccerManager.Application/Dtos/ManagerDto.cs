namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a manager profile for read-oriented consumers.
/// </summary>
public sealed record ManagerDto
{
    /// <summary>
    /// The identifier of the manager profile.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the Keycloak user this manager profile belongs to.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// The URL of the manager's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// The UTC date and time the manager profile was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the manager profile was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
