using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamNextMatch;

/// <summary>
/// Validates <see cref="GetLeagueTeamNextMatchRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamNextMatchValidator : AbstractValidator<GetLeagueTeamNextMatchRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamNextMatchValidator"/> class.
    /// </summary>
    public GetLeagueTeamNextMatchValidator()
    {
        // This query is always scoped to a league team, so a missing or zero LeagueTeamId must fail fast here, as a 400,
        // rather than reach the handler, where it would be a 403.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
    }
}
