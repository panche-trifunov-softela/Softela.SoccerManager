using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerInjury.GetLeagueTeamPlayerInjuries;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamPlayerInjuriesRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamPlayerInjuriesResponse
{
    /// <summary>
    /// The list of injuries of the requested squad row, the latest first.
    /// </summary>
    public required List<LeagueTeamPlayerInjuryDto> Data { get; init; }
}
