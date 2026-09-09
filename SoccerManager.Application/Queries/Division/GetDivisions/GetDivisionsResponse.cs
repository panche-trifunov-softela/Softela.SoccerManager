using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Division.GetDivisions;

/// <summary>
/// Represents the result of a <see cref="GetDivisionsRequest"/> query.
/// </summary>
public sealed record GetDivisionsResponse
{
    /// <summary>
    /// The list of divisions belonging to the requested league.
    /// </summary>
    public required List<DivisionDto> Data { get; init; }
}
