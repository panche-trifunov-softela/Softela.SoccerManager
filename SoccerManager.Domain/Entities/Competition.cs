using SoccerManager.Domain.Enums;

namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a competition played within a league.
/// </summary>
public class Competition : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the league this competition belongs to.
    /// </summary>
    public int LeagueId { get; set; }

    /// <summary>
    /// Gets or sets the competition's name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the URL of the competition's logo image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the competition is domestic.
    /// </summary>
    public bool IsDomestic { get; set; }

    /// <summary>
    /// Gets or sets the competition's format.
    /// </summary>
    public CompetitionFormat Format { get; set; }

    /// <summary>
    /// Gets or sets the maximum age allowed to compete in the competition, or <see langword="null"/> when it has no
    /// age limit. When set, the value is between 16 and 23.
    /// </summary>
    public int? MaxAgeAllowed { get; set; }
}
