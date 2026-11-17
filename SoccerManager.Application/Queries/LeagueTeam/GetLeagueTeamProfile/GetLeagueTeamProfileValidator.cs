using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamProfile;

/// <summary>
/// Validates <see cref="GetLeagueTeamProfileRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamProfileValidator : AbstractValidator<GetLeagueTeamProfileRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamProfileValidator"/> class.
    /// </summary>
    public GetLeagueTeamProfileValidator()
    {
        // This query is always scoped to a league team, so a missing, zero or negative LeagueTeamId must fail fast here, as a
        // 400, rather than reach the handler, where it would be a 404.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
    }
}
