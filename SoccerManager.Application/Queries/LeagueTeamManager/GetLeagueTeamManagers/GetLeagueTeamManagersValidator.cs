using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeamManager.GetLeagueTeamManagers;

/// <summary>
/// Validates <see cref="GetLeagueTeamManagersRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamManagersValidator : AbstractValidator<GetLeagueTeamManagersRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamManagersValidator"/> class.
    /// </summary>
    public GetLeagueTeamManagersValidator()
    {
        // This query is always scoped to one team within one league, so a
        // missing or zero LeagueId or TeamId must fail fast rather than
        // silently returning nothing.
        RuleFor(x => x.LeagueId).GreaterThan(0);
        RuleFor(x => x.TeamId).GreaterThan(0);
    }
}
