using FluentValidation;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerLoan.AnswerLeagueTeamPlayerLoanBid;

/// <summary>
/// Validates <see cref="AnswerLeagueTeamPlayerLoanBidRequest"/> instances.
/// </summary>
public sealed class AnswerLeagueTeamPlayerLoanBidValidator : AbstractValidator<AnswerLeagueTeamPlayerLoanBidRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnswerLeagueTeamPlayerLoanBidValidator"/> class.
    /// </summary>
    public AnswerLeagueTeamPlayerLoanBidValidator()
    {
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
        RuleFor(x => x.PlayerId).GreaterThan(0);
        RuleFor(x => x.BidId).GreaterThan(0);

        // A zero fails IsInEnum, and only Accepted or Rejected is an answer (Pending and Completed are not).
        RuleFor(x => x.Status)
            .IsInEnum()
            .Must(status => status is TransferStatus.Accepted or TransferStatus.Rejected).WithMessage("A bid can only be answered with Accepted or Rejected.");
    }
}
