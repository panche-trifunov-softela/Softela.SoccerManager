using FluentValidation;

namespace SoccerManager.Application.Commands.Competition.UpdateCompetition;

/// <summary>
/// Validates <see cref="UpdateCompetitionRequest"/> instances.
/// </summary>
public sealed class UpdateCompetitionValidator : AbstractValidator<UpdateCompetitionRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCompetitionValidator"/> class.
    /// </summary>
    public UpdateCompetitionValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LogoUrl).MaximumLength(500);
        RuleFor(x => x.Format).IsInEnum();

        // NULL means no age limit (a senior competition); when set, the range is 16-23.
        RuleFor(x => x.MaxAgeAllowed).InclusiveBetween(16, 23).When(x => x.MaxAgeAllowed.HasValue);

        // Zero is legitimate throughout: a knockout cup has no tier, and the top
        // competition promotes nobody while the bottom relegates nobody.
        RuleFor(x => x.Order).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TeamsPromoted).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TeamsRelegated).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TeamsInPlayoffs).GreaterThanOrEqualTo(0);
    }
}
