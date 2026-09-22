using FluentValidation;

namespace SoccerManager.Application.Commands.Competition.CreateCompetition;

/// <summary>
/// Validates <see cref="CreateCompetitionRequest"/> instances.
/// </summary>
public sealed class CreateCompetitionValidator : AbstractValidator<CreateCompetitionRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCompetitionValidator"/> class.
    /// </summary>
    public CreateCompetitionValidator()
    {
        // Shape-only: that the league exists is enforced by FK_Competitions_Leagues.
        RuleFor(x => x.LeagueId).GreaterThan(0);

        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LogoUrl).MaximumLength(500);
        RuleFor(x => x.Format).IsInEnum();

        // Null means no age limit, so the range is only checked when a value was supplied.
        RuleFor(x => x.MaxAgeAllowed).InclusiveBetween(16, 23).When(x => x.MaxAgeAllowed.HasValue);

        // Zero is legitimate throughout: a knockout cup has no tier, and the top
        // competition promotes nobody while the bottom relegates nobody.
        RuleFor(x => x.Order).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TeamsPromoted).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TeamsRelegated).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TeamsInPlayoffs).GreaterThanOrEqualTo(0);
    }
}
