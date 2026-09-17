using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Match.GetMatches;

/// <summary>
/// Represents the result of a <see cref="GetMatchesRequest"/> query.
/// </summary>
public sealed record GetMatchesResponse
{
    /// <summary>
    /// The list of all matches.
    /// </summary>
    public required List<MatchDto> Data { get; init; }
}
