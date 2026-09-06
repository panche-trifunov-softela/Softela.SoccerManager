namespace SoccerManager.Domain.Entities;

/// <summary>
/// Provides the identity and audit properties shared by every persisted entity.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the entity's unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who created the entity.
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the UTC moment the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user who last modified the entity.
    /// </summary>
    public Guid ModifiedBy { get; set; }

    /// <summary>
    /// Gets or sets the UTC moment the entity was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; set; }
}
