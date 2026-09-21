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
        RuleFor(x => x.PositionId).GreaterThan(0);
        RuleFor(x => x.SlotNumber).InclusiveBetween(1, 11);
    }
}
