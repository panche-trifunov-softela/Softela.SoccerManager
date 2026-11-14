using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchPlayerAbsence.GetMatchPlayerAbsenceById;

/// <summary>
/// Represents the result of a <see cref="GetMatchPlayerAbsenceByIdRequest"/> query.
/// </summary>
public sealed record GetMatchPlayerAbsenceByIdResponse
{
    /// <summary>
    /// The requested match player absence.
    /// </summary>
    public required MatchPlayerAbsenceDto Data { get; init; }
}
