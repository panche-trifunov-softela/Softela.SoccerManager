using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetLeagueTeamManagerApplications;

/// <summary>
/// Validates <see cref="GetLeagueTeamManagerApplicationsRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamManagerApplicationsValidator : AbstractValidator<GetLeagueTeamManagerApplicationsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamManagerApplicationsValidator"/> class.
    /// </summary>
    public GetLeagueTeamManagerApplicationsValidator()
    {
        // This query is always scoped to one team within one league, so a
        // missing or zero LeagueId or TeamId must fail fast rather than
        // silently returning nothing.
        RuleFor(x => x.LeagueId).GreaterThan(0);
        RuleFor(x => x.TeamId).GreaterThan(0);
    }
}
