using FluentValidation;

namespace SoccerManager.Application.Commands.NationalTeamMatch.UpdateNationalTeamMatch;

/// <summary>
/// Validates <see cref="UpdateNationalTeamMatchRequest"/> instances.
/// </summary>
public sealed class UpdateNationalTeamMatchValidator : AbstractValidator<UpdateNationalTeamMatchRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateNationalTeamMatchValidator"/> class.
    /// </summary>
    public UpdateNationalTeamMatchValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.SeasonId).GreaterThan(0);
        RuleFor(x => x.HomeNationalTeamId).GreaterThan(0);
        RuleFor(x => x.AwayNationalTeamId).GreaterThan(0);

        // Mirrors CK_NationalTeamMatches_DifferentNations, so a breach is a 400 rather than a database error.
        RuleFor(x => x.AwayNationalTeamId)
            .NotEqual(x => x.HomeNationalTeamId)
            .WithMessage("A national team cannot play itself.");

        // Mirrors CK_NationalTeamMatches_StageKnown, so a breach is a 400 rather than a database error.
        RuleFor(x => x.Stage).IsInEnum();

        // Mirrors CK_NationalTeamMatches_GroupNumberPositive; the column is null where the stage has no groups.
        RuleFor(x => x.GroupNumber).GreaterThanOrEqualTo((byte)1).When(x => x.GroupNumber.HasValue);

        // Mirrors CK_NationalTeamMatches_RoundNumberPositive; the column is null where the stage has no matchdays.
        RuleFor(x => x.RoundNumber).GreaterThanOrEqualTo((byte)1).When(x => x.RoundNumber.HasValue);

        // Mirrors CK_NationalTeamMatches_LegFirstOrSecond; the column is null for a single match.
        RuleFor(x => x.Leg).InclusiveBetween((byte)1, (byte)2).When(x => x.Leg.HasValue);

        // Mirrors CK_NationalTeamMatches_FinishedHasStarted, so a breach is a 400 rather than a database error.
        RuleFor(x => x.IsStarted)
            .Equal(true)
            .WithMessage("A finished match must have started.")
            .When(x => x.IsFinished);
    }
}
