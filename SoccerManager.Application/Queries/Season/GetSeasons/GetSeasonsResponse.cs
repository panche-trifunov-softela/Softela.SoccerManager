using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Season.GetSeasons;

/// <summary>
/// Represents the result of a <see cref="GetSeasonsRequest"/> query.
/// </summary>
public sealed record GetSeasonsResponse
{
    /// <summary>
    /// The list of seasons belonging to the requested league.
    /// </summary>
    public required List<SeasonDto> Data { get; init; }
}
