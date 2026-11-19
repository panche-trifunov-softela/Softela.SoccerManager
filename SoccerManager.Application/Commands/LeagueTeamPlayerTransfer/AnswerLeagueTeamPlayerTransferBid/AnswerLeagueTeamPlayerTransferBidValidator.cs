using FluentValidation;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.AnswerLeagueTeamPlayerTransferBid;

/// <summary>
/// Validates <see cref="AnswerLeagueTeamPlayerTransferBidRequest"/> instances.
/// </summary>
public sealed class AnswerLeagueTeamPlayerTransferBidValidator : AbstractValidator<AnswerLeagueTeamPlayerTransferBidRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnswerLeagueTeamPlayerTransferBidValidator"/> class.
    /// </summary>
    public AnswerLeagueTeamPlayerTransferBidValidator()
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
