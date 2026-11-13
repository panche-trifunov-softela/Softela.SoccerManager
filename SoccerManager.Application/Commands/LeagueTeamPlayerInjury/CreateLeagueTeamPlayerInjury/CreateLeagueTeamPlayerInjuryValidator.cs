using FluentValidation;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerInjury.CreateLeagueTeamPlayerInjury;

/// <summary>
/// Validates <see cref="CreateLeagueTeamPlayerInjuryRequest"/> instances.
/// </summary>
public sealed class CreateLeagueTeamPlayerInjuryValidator : AbstractValidator<CreateLeagueTeamPlayerInjuryRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamPlayerInjuryValidator"/> class.
    /// </summary>
    public CreateLeagueTeamPlayerInjuryValidator()
    {
        RuleFor(x => x.LeagueTeamPlayerId).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.InjuryStartDate).NotEmpty();

        // Mirrors CK_LeagueTeamPlayerInjuries_EndNotBeforeStart, so a breach is a 400 rather than a database error.
        RuleFor(x => x.InjuryEndDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.InjuryStartDate)
            .WithMessage("The injury cannot end before it starts.");

        RuleFor(x => x.InjuryScale).IsInEnum();
    }
}
