using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerLoan.GetLeagueTeamPlayerLoans;

/// <summary>
/// Validates <see cref="GetLeagueTeamPlayerLoansRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamPlayerLoansValidator : AbstractValidator<GetLeagueTeamPlayerLoansRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayerLoansValidator"/> class.
    /// </summary>
    public GetLeagueTeamPlayerLoansValidator()
    {
        // This query is always scoped to a squad row, so a missing or zero
        // LeagueTeamPlayerId must fail fast here rather than silently returning nothing.
        RuleFor(x => x.LeagueTeamPlayerId).GreaterThan(0);
    }
}
