using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.MatchPlayerAbsence.CreateMatchPlayerAbsence;

/// <summary>
/// Represents a request to create a new match player absence.
/// </summary>
public sealed record CreateMatchPlayerAbsenceRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the match the player misses.
    /// </summary>
    public int MatchId { get; init; }

    /// <summary>
    /// The identifier of the league team (the club in its league) the player is missing from.
    /// </summary>
    public int LeagueTeamId { get; init; }

    /// <summary>
    /// The identifier of the player who is absent.
    /// </summary>
    public int PlayerId { get; init; }

    /// <summary>
    /// Why the player misses the match.
    /// </summary>
    public MatchAbsenceReason Reason { get; init; }
}
