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
    /// The date and time the league was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>
    /// The date and time the league was last modified.
    /// </summary>
    public DateTimeOffset ModifiedAt { get; init; }
}
