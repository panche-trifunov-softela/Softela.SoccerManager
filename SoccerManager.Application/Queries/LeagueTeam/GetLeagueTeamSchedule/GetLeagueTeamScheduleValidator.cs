using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSchedule;

/// <summary>
/// Validates <see cref="GetLeagueTeamScheduleRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamScheduleValidator : AbstractValidator<GetLeagueTeamScheduleRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamScheduleValidator"/> class.
    /// </summary>
    public GetLeagueTeamScheduleValidator()
    {
        // This query is always scoped to a league team, so a missing or zero LeagueTeamId
        // must fail fast here rather than silently returning nothing.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
    }
}
