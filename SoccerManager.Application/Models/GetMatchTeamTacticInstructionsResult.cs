using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Models;

/// <summary>
/// One row returned by the <c>dbo.GetMatchTeamTacticInstructions</c> and <c>dbo.UpdateMatchTeamTacticInstructions</c>
/// stored procedures: a club's seven instructions for a match. Kept separate from <see cref="Dtos.MatchInstructionsDto"/>
/// because Dapper populates a result by setting properties, and the Dto is an init-only record that cannot be a Dapper target.
/// </summary>
public class GetMatchTeamTacticInstructionsResult
{
    /// <summary>
    /// The team's mentality.
    /// </summary>
    public Mentality Mentality { get; set; }

    /// <summary>
    /// The team's tempo.
    /// </summary>
    public Tempo Tempo { get; set; }

    /// <summary>
    /// The team's passing style.
    /// </summary>
    public Passing Passing { get; set; }

    /// <summary>
    /// The width the team plays with.
    /// </summary>
    public Width Width { get; set; }

    /// <summary>
    /// The height at which the team presses.
    /// </summary>
    public Pressing Pressing { get; set; }

    /// <summary>
    /// How aggressively the team tackles.
    /// </summary>
    public Tackling Tackling { get; set; }

    /// <summary>
    /// The side of the pitch the team directs its attacks through.
    /// </summary>
    public AttackingSide AttackingSide { get; set; }
}
