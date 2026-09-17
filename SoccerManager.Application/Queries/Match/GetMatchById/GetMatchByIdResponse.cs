using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Match.GetMatchById;

/// <summary>
/// Represents the result of a <see cref="GetMatchByIdRequest"/> query.
/// </summary>
public sealed record GetMatchByIdResponse
{
    /// <summary>
    /// The requested match.
    /// </summary>
    public required MatchDto Data { get; init; }
}
