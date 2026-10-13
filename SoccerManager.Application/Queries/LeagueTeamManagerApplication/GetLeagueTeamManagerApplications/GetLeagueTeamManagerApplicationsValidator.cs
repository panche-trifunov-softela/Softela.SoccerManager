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
        // This query is always scoped to one league roster entry, so a
        // missing or zero LeagueTeamId must fail fast rather than
        // silently returning nothing.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
    }
}
