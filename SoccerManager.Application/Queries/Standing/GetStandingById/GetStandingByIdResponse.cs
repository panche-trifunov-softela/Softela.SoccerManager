using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Standing.GetStandingById;

/// <summary>
/// Represents the result of a <see cref="GetStandingByIdRequest"/> query.
/// </summary>
public sealed record GetStandingByIdResponse
{
    /// <summary>
    /// The requested standing.
    /// </summary>
    public required StandingDto Data { get; init; }
}
