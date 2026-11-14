using MediatR;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.MatchPlayerAbsence.UpdateMatchPlayerAbsence;

/// <summary>
/// Represents a request to update an existing match player absence.
/// </summary>
public sealed record UpdateMatchPlayerAbsenceRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the match player absence to update.
    /// </summary>
    public int Id { get; init; }

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
