using MediatR;

namespace SoccerManager.Application.Commands.Match.UpdateMatch;

/// <summary>
/// Represents a request to update an existing match.
/// </summary>
public sealed record UpdateMatchRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the match to update.
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
    /// The match commentary as a JSON document, if any has been set.
    /// </summary>
    public string? Commentary { get; init; }
}
