using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerInjuries;

/// <summary>
/// Represents the result of a <see cref="GetLeaguePlayerInjuriesRequest"/> query.
/// </summary>
public sealed record GetLeaguePlayerInjuriesResponse
{
    /// <summary>
    /// The requested player's injuries in the league, newest first.
    /// </summary>
    public required List<PlayerInjuryDto> Data { get; init; }
}
