using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamProfile;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamProfileRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamProfileResponse
{
    /// <summary>
    /// The requested league team's profile.
    /// </summary>
    public required LeagueTeamProfileDto Data { get; init; }
}
