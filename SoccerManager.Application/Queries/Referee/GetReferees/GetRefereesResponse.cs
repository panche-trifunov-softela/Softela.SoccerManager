using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Referee.GetReferees;

/// <summary>
/// Represents the result of a <see cref="GetRefereesRequest"/> query.
/// </summary>
public sealed record GetRefereesResponse
{
    /// <summary>
    /// The list of all referees.
    /// </summary>
    public required List<RefereeDto> Data { get; init; }
}
