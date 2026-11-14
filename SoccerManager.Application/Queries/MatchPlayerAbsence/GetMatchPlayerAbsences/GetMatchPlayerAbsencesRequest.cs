using MediatR;

namespace SoccerManager.Application.Queries.MatchPlayerAbsence.GetMatchPlayerAbsences;

/// <summary>
/// Represents a request to retrieve every player absence recorded for a match.
/// </summary>
public sealed record GetMatchPlayerAbsencesRequest : IRequest<GetMatchPlayerAbsencesResponse>
{
    /// <summary>
    /// The identifier of the match whose player absences are being requested.
    /// </summary>
    public int MatchId { get; init; }
}
