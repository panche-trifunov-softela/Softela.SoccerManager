using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerProfile;

/// <summary>
/// Represents the result of a <see cref="GetLeaguePlayerProfileRequest"/> query.
/// </summary>
public sealed record GetLeaguePlayerProfileResponse
{
    /// <summary>
    /// The requested player's profile in the league.
    /// </summary>
    public required SquadPlayerProfileDto Data { get; init; }
}
