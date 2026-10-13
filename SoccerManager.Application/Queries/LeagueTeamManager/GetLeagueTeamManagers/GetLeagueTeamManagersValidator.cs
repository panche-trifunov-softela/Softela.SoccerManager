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
        // This query is always scoped to one league roster entry, so a
        // missing or zero LeagueTeamId must fail fast rather than
        // silently returning nothing.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
    }
}
