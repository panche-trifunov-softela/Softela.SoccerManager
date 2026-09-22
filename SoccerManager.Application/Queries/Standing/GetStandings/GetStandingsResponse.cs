using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Standing.GetStandings;

/// <summary>
/// Represents the result of a <see cref="GetStandingsRequest"/> query.
/// </summary>
public sealed record GetStandingsResponse
{
    /// <summary>
    /// The list of standings belonging to the requested competition and season.
    /// </summary>
    public required List<StandingDto> Data { get; init; }
}
