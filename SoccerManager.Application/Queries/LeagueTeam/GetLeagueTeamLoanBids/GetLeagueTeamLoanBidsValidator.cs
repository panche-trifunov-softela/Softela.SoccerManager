using FluentValidation;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamLoanBids;

/// <summary>
/// Validates <see cref="GetLeagueTeamLoanBidsRequest"/> instances.
/// </summary>
public sealed class GetLeagueTeamLoanBidsValidator : AbstractValidator<GetLeagueTeamLoanBidsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamLoanBidsValidator"/> class.
    /// </summary>
    public GetLeagueTeamLoanBidsValidator()
    {
        // A missing, zero or negative id fails fast here, as a 400, before the access check, where it would be a 403.
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
    }
}
