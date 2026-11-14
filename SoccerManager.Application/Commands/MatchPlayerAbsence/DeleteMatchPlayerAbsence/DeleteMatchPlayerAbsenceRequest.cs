using MediatR;

namespace SoccerManager.Application.Commands.MatchPlayerAbsence.DeleteMatchPlayerAbsence;

/// <summary>
/// Represents a request to delete an existing match player absence.
/// </summary>
public sealed record DeleteMatchPlayerAbsenceRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the match player absence to delete.
    /// </summary>
    public int Id { get; init; }
}
