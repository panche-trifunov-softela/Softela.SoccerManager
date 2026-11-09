using MediatR;
using SoccerManager.Application.Dtos;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.Match.DeleteMatchSubstitution;

/// <summary>
/// Represents a request to delete a club's substitution for a match at a minute and situation, its lineup and its instructions.
/// </summary>
public sealed record DeleteMatchSubstitutionRequest : IRequest<IReadOnlyList<MatchSubstitutionDto>>
{
    /// <summary>
    /// The identifier of the match the substitution is for.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) whose substitution to delete.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The minute of the substitution to delete: 45, 60 or 75.
    /// </summary>
    public int MinuteAt { get; init; }

    /// <summary>
    /// The situation of the substitution to delete.
    /// </summary>
    public Situation Situation { get; init; }
}
