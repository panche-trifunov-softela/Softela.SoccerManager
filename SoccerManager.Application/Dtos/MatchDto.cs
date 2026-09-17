namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a match for read-oriented consumers.
/// </summary>
public sealed record MatchDto
{
    /// <summary>
    /// The identifier of the match.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the season the match is played in.
    /// </summary>
    public int SeasonId { get; init; }

    /// <summary>
    /// The identifier of the division the match is played in.
    /// </summary>
    public int DivisionId { get; init; }

    /// <summary>
    /// The identifier of the home team.
    /// </summary>
    public int HomeTeamId { get; init; }

    /// <summary>
    /// The identifier of the away team.
    /// </summary>
    public int AwayTeamId { get; init; }

    /// <summary>
    /// The identifier of the match's referee.
    /// </summary>
    public int RefereeId { get; init; }

    /// <summary>
    /// The UTC moment the match starts.
    /// </summary>
    public DateTime StartDateTime { get; init; }

    /// <summary>
    /// The match commentary as a JSON document, or <see langword="null"/> when there is none.
    /// </summary>
    public string? Commentary { get; init; }

    /// <summary>
    /// The UTC date and time the match was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the match was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
