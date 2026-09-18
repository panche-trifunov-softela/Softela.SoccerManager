using MediatR;

namespace SoccerManager.Application.Commands.Match.CreateMatch;

/// <summary>
/// Represents a request to create a new match.
/// </summary>
public sealed record CreateMatchRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the season the match is played in.
    /// </summary>
    public int SeasonId { get; init; }

    /// <summary>
    /// The identifier of the division the match is played in.
    /// </summary>
    public int DivisionId { get; init; }

    /// <summary>
    /// The identifier of the match's referee.
    /// </summary>
    public int RefereeId { get; init; }

    /// <summary>
    /// The UTC moment the match starts.
    /// </summary>
    public DateTime StartDateTime { get; init; }

    /// <summary>
    /// The match commentary as a JSON document, if any has been set.
    /// </summary>
    public string? Commentary { get; init; }

    /// <summary>
    /// The number of spectators at the match; zero until it has been played.
    /// </summary>
    public int Attendance { get; init; }

    /// <summary>
    /// Whether the match has started.
    /// </summary>
    public bool IsStarted { get; init; }
}
