using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchPlayerAbsence.GetMatchPlayerAbsences;

/// <summary>
/// Represents the result of a <see cref="GetMatchPlayerAbsencesRequest"/> query.
/// </summary>
public sealed record GetMatchPlayerAbsencesResponse
{
    /// <summary>
    /// The list of player absences recorded for the requested match.
    /// </summary>
    public required List<MatchPlayerAbsenceDto> Data { get; init; }
}
