using FluentValidation;

namespace SoccerManager.Application.Commands.NationalTeamMatch.CreateNationalTeamMatch;

/// <summary>
/// Validates <see cref="CreateNationalTeamMatchRequest"/> instances.
/// </summary>
public sealed class CreateNationalTeamMatchValidator : AbstractValidator<CreateNationalTeamMatchRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateNationalTeamMatchValidator"/> class.
    /// </summary>
    public CreateNationalTeamMatchValidator()
    {
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
    }
}
