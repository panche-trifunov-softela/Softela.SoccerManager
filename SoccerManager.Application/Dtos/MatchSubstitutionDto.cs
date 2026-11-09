using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents one of a club's substitutions for a match: the lineup and instructions it switches to at one minute when the
/// score is in one situation.
/// </summary>
public sealed record MatchSubstitutionDto
{
    /// <summary>
    /// The minute of the match the substitution takes effect at: 45, 60 or 75.
    /// </summary>
    public int MinuteAt { get; init; }

    /// <summary>
    /// The state of the score the substitution is for.
    /// </summary>
    public Situation Situation { get; init; }

    /// <summary>
    /// The lineup the club switches to; each slot says how the substitution moves its player compared with the main lineup.
    /// </summary>
    public required MatchLineupDto Lineup { get; init; }

    /// <summary>
    /// The instructions the club switches to.
    /// </summary>
    public required MatchInstructionsDto Instructions { get; init; }
}
