using FluentValidation;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerLoan.UpdateLeagueTeamPlayerLoan;

/// <summary>
/// Validates <see cref="UpdateLeagueTeamPlayerLoanRequest"/> instances.
/// </summary>
public sealed class UpdateLeagueTeamPlayerLoanValidator : AbstractValidator<UpdateLeagueTeamPlayerLoanRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueTeamPlayerLoanValidator"/> class.
    /// </summary>
    public UpdateLeagueTeamPlayerLoanValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.LeagueTeamPlayerId).GreaterThan(0);
        RuleFor(x => x.ToLeagueTeamId).GreaterThan(0);
        RuleFor(x => x.SeasonId).GreaterThan(0);

        // PrecisionScale matches the DECIMAL(18,2) columns, so an over-precise value is a 400
        // rather than a silent rounding at the database.
        RuleFor(x => x.LoanFee).GreaterThanOrEqualTo(0m).PrecisionScale(18, 2, false);
        RuleFor(x => x.BuyOptionValue).GreaterThanOrEqualTo(0m).PrecisionScale(18, 2, false).When(x => x.BuyOptionValue.HasValue);

        // Mirrors CK_LeagueTeamPlayerLoans_SeasonsLength and CK_LeagueTeamPlayerLoans_SalaryCoverageInPercent,
        // so a breach is a 400 rather than a database error.
        RuleFor(x => x.SeasonsLength).InclusiveBetween(1, 2);
        RuleFor(x => x.SalaryCoverageInPercent).InclusiveBetween(0, 100);

        // A zero fails here, as the values of TransferStatus start at 1.
        RuleFor(x => x.Status).IsInEnum();
    }
}
