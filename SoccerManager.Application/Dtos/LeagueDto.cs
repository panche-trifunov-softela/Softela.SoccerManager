namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a league for read-oriented consumers.
/// </summary>
public sealed record LeagueDto
{
    /// <summary>
    /// The identifier of the league.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The name of the league.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The UTC date and time the league was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the league was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
