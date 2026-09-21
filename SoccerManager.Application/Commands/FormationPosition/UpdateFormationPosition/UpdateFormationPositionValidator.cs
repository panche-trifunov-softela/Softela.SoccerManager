using FluentValidation;

namespace SoccerManager.Application.Commands.FormationPosition.UpdateFormationPosition;

/// <summary>
/// Validates <see cref="UpdateFormationPositionRequest"/> instances.
/// </summary>
public sealed class UpdateFormationPositionValidator : AbstractValidator<UpdateFormationPositionRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateFormationPositionValidator"/> class.
    /// </summary>
    public UpdateFormationPositionValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.FormationId).GreaterThan(0);
        RuleFor(x => x.PositionId).GreaterThan(0);
        RuleFor(x => x.SlotNumber).InclusiveBetween(1, 11);
    }
}
