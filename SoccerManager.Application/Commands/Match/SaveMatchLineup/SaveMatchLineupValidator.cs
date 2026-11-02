using FluentValidation;

namespace SoccerManager.Application.Commands.Match.SaveMatchLineup;

/// <summary>
/// Validates <see cref="SaveMatchLineupRequest"/> instances.
/// </summary>
public sealed class SaveMatchLineupValidator : AbstractValidator<SaveMatchLineupRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SaveMatchLineupValidator"/> class.
    /// </summary>
    public SaveMatchLineupValidator()
    {
        RuleFor(x => x.MatchId).GreaterThan(0);
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);
        RuleFor(x => x.FormationId).GreaterThan(0).When(x => x.FormationId.HasValue);

        // Shape-only: whether the slot, the player or the formation exists is for the handler, which reads the database.
        RuleFor(x => x.SlotNumber).InclusiveBetween(1, 19).When(x => x.SlotNumber.HasValue);
        RuleFor(x => x.PlayerId).GreaterThan(0).When(x => x.PlayerId.HasValue);

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
