using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Referee.GetRefereeById;

/// <summary>
/// Represents the result of a <see cref="GetRefereeByIdRequest"/> query.
/// </summary>
public sealed record GetRefereeByIdResponse
{
    /// <summary>
    /// The requested referee.
    /// </summary>
    public required RefereeDto Data { get; init; }
}
