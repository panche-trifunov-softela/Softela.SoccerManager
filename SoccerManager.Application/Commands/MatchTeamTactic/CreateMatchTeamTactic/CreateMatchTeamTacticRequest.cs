using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.MatchTeamTactic.CreateMatchTeamTactic;

/// <summary>
/// Represents a request to create a new match team tactic.
/// </summary>
public sealed record CreateMatchTeamTacticRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the match the tactic applies to.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The identifier of the team the tactic belongs to.
    /// </summary>
    public int TeamId { get; init; }

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

    /// <summary>
    /// The identifier of the player who takes penalties.
    /// </summary>
    public int PenaltyTakerPlayerId { get; init; }

    /// <summary>
    /// The identifier of the player who takes free kicks.
    /// </summary>
    public int FreeKickTakerPlayerId { get; init; }

    /// <summary>
    /// The identifier of the player who takes corner kicks.
    /// </summary>
    public int CornerKickTakerPlayerId { get; init; }

    /// <summary>
    /// The identifier of the player who captains the team.
    /// </summary>
    public int CaptainPlayerId { get; init; }
}
