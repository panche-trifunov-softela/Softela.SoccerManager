using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents a club's seven instructions for a match: its mentality, tempo, passing, width, pressing, tackling and
/// attacking side.
/// </summary>
public sealed record MatchInstructionsDto
{
    /// <summary>
    /// The identifier of the match.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) the instructions belong to.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The team's mentality.
    /// </summary>
    public Mentality Mentality { get; init; }

    /// <summary>
    /// The team's tempo.
    /// </summary>
    public Tempo Tempo { get; init; }

    /// <summary>
    /// The team's passing style.
    /// </summary>
    public Passing Passing { get; init; }

    /// <summary>
    /// The width the team plays with.
    /// </summary>
    public Width Width { get; init; }

    /// <summary>
    /// The height at which the team presses.
    /// </summary>
    public Pressing Pressing { get; init; }

    /// <summary>
    /// How aggressively the team tackles.
    /// </summary>
    public Tackling Tackling { get; init; }

    /// <summary>
    /// The side of the pitch the team directs its attacks through.
    /// </summary>
    public AttackingSide AttackingSide { get; init; }
}
