using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Division.GetDivisionById;

/// <summary>
/// Represents the result of a <see cref="GetDivisionByIdRequest"/> query.
/// </summary>
public sealed record GetDivisionByIdResponse
{
    /// <summary>
    /// The requested division.
    /// </summary>
    public required DivisionDto Data { get; init; }
}
