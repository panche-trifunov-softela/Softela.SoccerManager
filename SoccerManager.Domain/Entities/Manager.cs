namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a manager profile linked to a Keycloak user.
/// </summary>
public class Manager : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the Keycloak user this manager profile belongs to.
    /// There is deliberately no foreign key here: Keycloak's store is a separate database, so the link is by convention only.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the URL of the manager's image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? ImageUrl { get; set; }
}
