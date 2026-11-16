using FluentValidation;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerLoan.PlaceLeagueTeamPlayerLoanBid;

/// <summary>
/// Validates <see cref="PlaceLeagueTeamPlayerLoanBidRequest"/> instances.
/// </summary>
public sealed class PlaceLeagueTeamPlayerLoanBidValidator : AbstractValidator<PlaceLeagueTeamPlayerLoanBidRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlaceLeagueTeamPlayerLoanBidValidator"/> class.
    /// </summary>
    public PlaceLeagueTeamPlayerLoanBidValidator()
    {
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
        RuleFor(x => x.PlayerId).GreaterThan(0);

        // PrecisionScale matches the DECIMAL(18,2) columns, so an over-precise value is a 400
        // rather than a silent rounding at the database.
        RuleFor(x => x.LoanFee).GreaterThanOrEqualTo(0m).PrecisionScale(18, 2, false);
        RuleFor(x => x.BuyOptionValue).GreaterThanOrEqualTo(0m).PrecisionScale(18, 2, false).When(x => x.BuyOptionValue.HasValue);

        // Mirrors CK_LeagueTeamPlayerLoans_SeasonsLength, so a breach is a 400 rather than a database error.
        RuleFor(x => x.SeasonsLength).InclusiveBetween(1, 2);
    }
}
