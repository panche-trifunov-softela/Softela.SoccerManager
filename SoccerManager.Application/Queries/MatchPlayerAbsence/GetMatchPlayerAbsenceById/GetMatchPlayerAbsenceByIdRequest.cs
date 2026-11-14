using MediatR;

namespace SoccerManager.Application.Queries.MatchPlayerAbsence.GetMatchPlayerAbsenceById;

/// <summary>
/// Represents a request to retrieve a single match player absence by identifier.
/// </summary>
public sealed record GetMatchPlayerAbsenceByIdRequest : IRequest<GetMatchPlayerAbsenceByIdResponse>
{
    /// <summary>
    /// The identifier of the match player absence to retrieve.
    /// </summary>
    public int Id { get; init; }
}
