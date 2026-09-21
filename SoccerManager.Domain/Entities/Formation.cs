namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a formation a team can line up in.
/// </summary>
public class Formation : BaseEntity
{
    /// <summary>
    /// Gets or sets the formation's name.
    /// </summary>
    public required string Name { get; set; }
}
