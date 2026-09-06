using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.League.GetLeagues;

/// <summary>
/// Represents the result of a <see cref="GetLeaguesRequest"/> query.
/// </summary>
public sealed record GetLeaguesResponse
{
    /// <summary>
    /// The list of all leagues.
    /// </summary>
    public required List<LeagueDto> Data { get; init; }
}
