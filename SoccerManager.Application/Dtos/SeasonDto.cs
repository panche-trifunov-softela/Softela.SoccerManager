namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a season for read-oriented consumers.
/// </summary>
public sealed record SeasonDto
{
    /// <summary>
    /// The identifier of the season.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the league the season belongs to.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The number of the season.
    /// </summary>
    public int SeasonNumber { get; init; }

    /// <summary>
    /// The UTC date and time the season was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the season was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
