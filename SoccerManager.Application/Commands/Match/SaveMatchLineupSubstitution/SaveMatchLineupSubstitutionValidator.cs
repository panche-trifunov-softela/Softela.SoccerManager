using FluentValidation;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.Match.SaveMatchLineupSubstitution;

/// <summary>
/// Validates <see cref="SaveMatchLineupSubstitutionRequest"/> instances.
/// </summary>
public sealed class SaveMatchLineupSubstitutionValidator : AbstractValidator<SaveMatchLineupSubstitutionRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SaveMatchLineupSubstitutionValidator"/> class.
    /// </summary>
    public SaveMatchLineupSubstitutionValidator()
    {
        RuleFor(x => x.MatchId).GreaterThan(0);
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);

        RuleFor(x => x.MinuteAt)
            .Must(minute => MatchSubstitutionLimits.Minutes.Contains(minute))
            .WithMessage($"Minute at must be {MatchSubstitutionLimits.MinutesInWords}.");

        // Situation starts at 1, so IsInEnum also refuses a zero or a missing value.
        RuleFor(x => x.Situation).IsInEnum();

        RuleFor(x => x.FormationId).GreaterThan(0).When(x => x.FormationId.HasValue);

        // Shape-only: whether the slot, the player or the formation exists is for the handler, which reads the database.
        RuleFor(x => x.SlotNumber).InclusiveBetween(1, 19).When(x => x.SlotNumber.HasValue);
        RuleFor(x => x.PlayerId).GreaterThan(0).When(x => x.PlayerId.HasValue);

        // Unlike the main lineup, a substitution cannot clear a slot, so a slot needs its player.
        RuleFor(x => x.PlayerId)
            .NotNull()
            .WithMessage(x => $"Choose the player for slot {x.SlotNumber}.")
            .When(x => x.SlotNumber.HasValue && !x.PlayerId.HasValue);

        // A player is put in a slot, so a player alone says nothing.
        RuleFor(x => x.PlayerId)
            .Null()
            .WithMessage("A player needs a slot number.")
            .When(x => x.PlayerId.HasValue && !x.SlotNumber.HasValue);

        // Exclusive with the rule above: it fires only when no player was sent either.
        RuleFor(x => x.SlotNumber)
            .NotNull()
            .WithMessage("Send a formation, a slot, or both.")
            .When(x => !x.FormationId.HasValue && !x.SlotNumber.HasValue && !x.PlayerId.HasValue);
    }
}
