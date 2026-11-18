using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.NationalTeamMatch.CreateNationalTeamMatch;

/// <summary>
/// Represents a request to create a new national team match.
/// </summary>
public sealed record CreateNationalTeamMatchRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the season the match is played in.
    /// </summary>
    public int SeasonId { get; init; }

    /// <summary>
    /// The identifier of the home national team.
    /// </summary>
    public int HomeNationalTeamId { get; init; }

    /// <summary>
    /// The identifier of the away national team. It differs from the home national team.
    /// </summary>
    public int AwayNationalTeamId { get; init; }

    /// <summary>
    /// The UTC moment the match starts.
    /// </summary>
    public DateTime StartDateTime { get; init; }

    /// <summary>
    /// The World Cup stage the match belongs to.
    /// </summary>
    public NationalTeamMatchStage Stage { get; init; }

    /// <summary>
    /// The group within the stage, counted from 1, or <see langword="null"/> when the stage has no groups.
    /// </summary>
    public byte? GroupNumber { get; init; }

    /// <summary>
    /// The matchday within the group, counted from 1, or <see langword="null"/> when it does not apply.
    /// </summary>
    public byte? RoundNumber { get; init; }

    /// <summary>
    /// The leg of a two-legged tie, 1 or 2, or <see langword="null"/> when the match is a single match.
    /// </summary>
    public byte? Leg { get; init; }

    /// <summary>
    /// Whether the match has started.
    /// </summary>
    public bool IsStarted { get; init; }
}
