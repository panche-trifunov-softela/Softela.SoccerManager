using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Match.GetMatchSubstitutions;

/// <summary>
/// Represents the result of a <see cref="GetMatchSubstitutionsRequest"/> query.
/// </summary>
public sealed record GetMatchSubstitutionsResponse
{
    /// <summary>
    /// The club's substitutions for the match, ordered by minute, then situation; empty when it has none.
    /// </summary>
    public required IReadOnlyList<MatchSubstitutionDto> Data { get; init; }
}
