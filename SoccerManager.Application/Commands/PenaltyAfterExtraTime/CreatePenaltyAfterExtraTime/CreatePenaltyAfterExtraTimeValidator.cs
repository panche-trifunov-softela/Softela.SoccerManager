using FluentValidation;

namespace SoccerManager.Application.Commands.PenaltyAfterExtraTime.CreatePenaltyAfterExtraTime;

/// <summary>
/// Validates <see cref="CreatePenaltyAfterExtraTimeRequest"/> instances.
/// </summary>
public sealed class CreatePenaltyAfterExtraTimeValidator : AbstractValidator<CreatePenaltyAfterExtraTimeRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePenaltyAfterExtraTimeValidator"/> class.
    /// </summary>
    public CreatePenaltyAfterExtraTimeValidator()
    {
        RuleFor(x => x.MatchId).GreaterThan(0);

        // Kicks are numbered from 1; the range matches CK_PenaltiesAfterExtraTime_KickNumber.
        RuleFor(x => x.KickNumber).GreaterThanOrEqualTo(1);

        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
        RuleFor(x => x.PlayerId).GreaterThan(0);
    }
}
