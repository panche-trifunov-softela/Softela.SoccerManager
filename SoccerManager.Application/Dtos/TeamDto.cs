using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a team for read-oriented consumers.
/// </summary>
public sealed record TeamDto
{
    /// <summary>
    /// The identifier of the team.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the team.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The identifier of the team's stadium, or <see langword="null"/> when it has none.
    /// </summary>
    public int? StadiumId { get; init; }

    /// <summary>
    /// The team's overall financial standing.
    /// </summary>
    public FinancialState FinancialState { get; init; }

    /// <summary>
    /// The URL of the team's jersey image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? JerseyUrl { get; init; }

    /// <summary>
    /// The URL of the team's logo image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? LogoUrl { get; init; }

    /// <summary>
    /// The UTC date and time the team was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the team was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
