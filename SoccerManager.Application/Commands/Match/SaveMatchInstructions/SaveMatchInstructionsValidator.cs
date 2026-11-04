using FluentValidation;

namespace SoccerManager.Application.Commands.Match.SaveMatchInstructions;

/// <summary>
/// Validates <see cref="SaveMatchInstructionsRequest"/> instances.
/// </summary>
public sealed class SaveMatchInstructionsValidator : AbstractValidator<SaveMatchInstructionsRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SaveMatchInstructionsValidator"/> class.
    /// </summary>
    public SaveMatchInstructionsValidator()
    {
        RuleFor(x => x.MatchId).GreaterThan(0);
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);

        // Every instruction enum starts at 1, so IsInEnum also refuses a zero.
        RuleFor(x => x.Mentality).IsInEnum().When(x => x.Mentality.HasValue);
        RuleFor(x => x.Tempo).IsInEnum().When(x => x.Tempo.HasValue);
        RuleFor(x => x.Passing).IsInEnum().When(x => x.Passing.HasValue);
        RuleFor(x => x.Width).IsInEnum().When(x => x.Width.HasValue);
        RuleFor(x => x.Pressing).IsInEnum().When(x => x.Pressing.HasValue);
        RuleFor(x => x.Tackling).IsInEnum().When(x => x.Tackling.HasValue);
        RuleFor(x => x.AttackingSide).IsInEnum().When(x => x.AttackingSide.HasValue);

        // A save changes something, so a body with no instruction says nothing.
        RuleFor(x => x.Mentality)
            .NotNull()
            .WithMessage("Send at least one instruction.")
            .When(x => !x.Mentality.HasValue && !x.Tempo.HasValue && !x.Passing.HasValue && !x.Width.HasValue
                && !x.Pressing.HasValue && !x.Tackling.HasValue && !x.AttackingSide.HasValue);
    }
}
