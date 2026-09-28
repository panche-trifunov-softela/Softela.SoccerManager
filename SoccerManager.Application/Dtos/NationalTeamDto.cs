namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a national team for read-oriented consumers.
/// </summary>
public sealed record NationalTeamDto
{
    /// <summary>
    /// The identifier of the national team.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the national team.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The identifier of the national team's stadium, or <see langword="null"/> when it has none.
    /// </summary>
    public int? StadiumId { get; init; }

    /// <summary>
    /// The URL of the national team's jersey image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? JerseyUrl { get; init; }

    /// <summary>
    /// The URL of the national team's logo image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? LogoUrl { get; init; }

    /// <summary>
    /// The UTC date and time the national team was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the national team was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
