using FluentValidation;

namespace SoccerManager.Application.Commands.FormationPosition.CreateFormationPosition;

/// <summary>
/// Validates <see cref="CreateFormationPositionRequest"/> instances.
/// </summary>
public sealed class CreateFormationPositionValidator : AbstractValidator<CreateFormationPositionRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateFormationPositionValidator"/> class.
    /// </summary>
    public CreateFormationPositionValidator()
    {
        RuleFor(x => x.FormationId).GreaterThan(0);

        // Shape-only: that the position exists is enforced by FK_FormationPositions_Positions, and a
        // reserve slot has none.
        RuleFor(x => x.PositionId).GreaterThan(0).When(x => x.PositionId.HasValue);

        // Mirrors CK_FormationPositions_PositionMatchesSlot, so a breach is a 400 rather than a database error.
        RuleFor(x => x.PositionId)
            .NotNull()
            .WithMessage("A starter slot (1 to 11) must have a position.")
            .When(x => x.SlotNumber is >= 1 and <= 11);

        RuleFor(x => x.PositionId)
            .Null()
            .WithMessage("A reserve slot (12 to 19) can't have a position.")
            .When(x => x.SlotNumber is >= 12 and <= 19);

        RuleFor(x => x.SlotNumber).InclusiveBetween(1, 19);
    }
}
