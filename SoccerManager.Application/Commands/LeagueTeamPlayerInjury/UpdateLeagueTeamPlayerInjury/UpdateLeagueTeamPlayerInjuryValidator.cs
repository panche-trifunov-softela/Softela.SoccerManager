using FluentValidation;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerInjury.UpdateLeagueTeamPlayerInjury;

/// <summary>
/// Validates <see cref="UpdateLeagueTeamPlayerInjuryRequest"/> instances.
/// </summary>
public sealed class UpdateLeagueTeamPlayerInjuryValidator : AbstractValidator<UpdateLeagueTeamPlayerInjuryRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueTeamPlayerInjuryValidator"/> class.
    /// </summary>
    public UpdateLeagueTeamPlayerInjuryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
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
