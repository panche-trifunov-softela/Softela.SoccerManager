using MediatR;
using SoccerManager.Application.Dtos;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.Match.SaveMatchInstructionsSubstitution;

/// <summary>
/// Represents a request to change one or more of a club's instructions in its substitution for a match at a minute and
/// situation; the ones not sent keep their value. The first change creates the substitution from the main lineup.
/// </summary>
public sealed record SaveMatchInstructionsSubstitutionRequest : IRequest<IReadOnlyList<MatchSubstitutionDto>>
{
    /// <summary>
    /// The identifier of the match the substitution is for. The route supplies it and overwrites a value in the body.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) whose substitution instructions to change.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The minute of the substitution to change: 45, 60 or 75.
    /// </summary>
    public int MinuteAt { get; init; }

    /// <summary>
    /// The situation of the substitution to change.
    /// </summary>
    public Situation Situation { get; init; }

    /// <summary>
    /// The team's mentality, or <see langword="null"/> to keep it.
    /// </summary>
    public Mentality? Mentality { get; init; }

    /// <summary>
    /// The team's tempo, or <see langword="null"/> to keep it.
    /// </summary>
    public Tempo? Tempo { get; init; }

    /// <summary>
    /// The team's passing style, or <see langword="null"/> to keep it.
    /// </summary>
    public Passing? Passing { get; init; }

    /// <summary>
    /// The width the team plays with, or <see langword="null"/> to keep it.
    /// </summary>
    public Width? Width { get; init; }

    /// <summary>
    /// The height at which the team presses, or <see langword="null"/> to keep it.
    /// </summary>
    public Pressing? Pressing { get; init; }

    /// <summary>
    /// How aggressively the team tackles, or <see langword="null"/> to keep it.
    /// </summary>
    public Tackling? Tackling { get; init; }

    /// <summary>
    /// The side of the pitch the team directs its attacks through, or <see langword="null"/> to keep it.
    /// </summary>
    public AttackingSide? AttackingSide { get; init; }
}
