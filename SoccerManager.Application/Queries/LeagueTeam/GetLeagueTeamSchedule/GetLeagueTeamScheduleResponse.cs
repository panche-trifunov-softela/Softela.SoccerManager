using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSchedule;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamScheduleRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamScheduleResponse
{
    /// <summary>
    /// The requested league team's matches in its League's latest season, earliest first.
    /// </summary>
    public required List<LeagueTeamScheduleMatchDto> Data { get; init; }
}
