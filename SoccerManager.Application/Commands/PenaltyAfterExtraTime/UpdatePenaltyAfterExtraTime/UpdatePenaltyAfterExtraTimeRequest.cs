using MediatR;

namespace SoccerManager.Application.Commands.PenaltyAfterExtraTime.UpdatePenaltyAfterExtraTime;

/// <summary>
/// Represents a request to update an existing penalty after extra time.
/// </summary>
public sealed record UpdatePenaltyAfterExtraTimeRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the penalty after extra time to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the match the shootout belongs to.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The place of the kick in the shootout, counted from 1. It is unique within the match.
    /// </summary>
    public int KickNumber { get; init; }

    /// <summary>
    /// The identifier of the league roster entry (the club in its league) that took the kick.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the player who took the kick.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// Whether the kick scored.
    /// </summary>
    public bool IsScored { get; init; }
}
