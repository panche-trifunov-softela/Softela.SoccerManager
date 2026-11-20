using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSchedule;

/// <summary>
/// Represents a request to retrieve the schedule of a league team.
/// </summary>
public sealed record GetLeagueTeamScheduleRequest : IRequest<GetLeagueTeamScheduleResponse>
{
    /// <summary>
    /// The identifier of the league team whose schedule is being requested.
    /// </summary>
    public int LeagueTeamId { get; init; }
}
