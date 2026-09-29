using SoccerManager.Domain.Enums;

namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a team that can compete in a league.
/// </summary>
public class Team : BaseEntity
{
    /// <summary>
    /// Gets or sets the team's name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the team's stadium, or <see langword="null"/> when it has none.
    /// </summary>
    public int? StadiumId { get; set; }

    /// <summary>
    /// Gets or sets the team's overall financial standing.
    /// </summary>
    public FinancialState FinancialState { get; set; }

    /// <summary>
    /// Gets or sets the URL of the team's jersey image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? JerseyUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL of the team's logo image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Gets or sets the team's Transfermarkt identifier, or <see langword="null"/> when it has none.
    /// </summary>
    public int? TransfermarktId { get; set; }
}
