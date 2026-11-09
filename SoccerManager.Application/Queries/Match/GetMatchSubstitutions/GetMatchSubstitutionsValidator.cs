using FluentValidation;

namespace SoccerManager.Application.Queries.Match.GetMatchSubstitutions;

/// <summary>
/// Validates <see cref="GetMatchSubstitutionsRequest"/> instances.
/// </summary>
public sealed class GetMatchSubstitutionsValidator : AbstractValidator<GetMatchSubstitutionsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchSubstitutionsValidator"/> class.
    /// </summary>
    public GetMatchSubstitutionsValidator()
    {
        RuleFor(x => x.MatchId).GreaterThan(0);

        // This query is always scoped to a league team, so a missing or zero LeagueTeamId must fail fast here, as a 400,
        // rather than reach the handler, where it would be a 403.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
    }
}
