using FluentValidation;

namespace SoccerManager.Application.Commands.PenaltyAfterExtraTime.UpdatePenaltyAfterExtraTime;

/// <summary>
/// Validates <see cref="UpdatePenaltyAfterExtraTimeRequest"/> instances.
/// </summary>
public sealed class UpdatePenaltyAfterExtraTimeValidator : AbstractValidator<UpdatePenaltyAfterExtraTimeRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePenaltyAfterExtraTimeValidator"/> class.
    /// </summary>
    public UpdatePenaltyAfterExtraTimeValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.MatchId).GreaterThan(0);

        // Kicks are numbered from 1; the range matches CK_PenaltiesAfterExtraTime_KickNumber.
        RuleFor(x => x.KickNumber).GreaterThanOrEqualTo(1);

        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
        RuleFor(x => x.PlayerId).GreaterThan(0);
    }
}
