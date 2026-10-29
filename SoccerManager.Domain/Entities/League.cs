namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a competition made up of teams playing against one another.
/// </summary>
public class League : BaseEntity
{
    /// <summary>
    /// Gets or sets the league's name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the manager who owns the league, or <see langword="null"/> when it has no owner.
    /// </summary>
    public int? OwnerManagerId { get; set; }
}
