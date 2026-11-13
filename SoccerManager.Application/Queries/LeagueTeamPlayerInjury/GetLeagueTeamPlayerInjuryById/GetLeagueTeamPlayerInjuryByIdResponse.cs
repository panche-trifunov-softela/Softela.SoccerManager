using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerInjury.GetLeagueTeamPlayerInjuryById;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamPlayerInjuryByIdRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamPlayerInjuryByIdResponse
{
    /// <summary>
    /// The requested injury.
    /// </summary>
    public required LeagueTeamPlayerInjuryDto Data { get; init; }
}
