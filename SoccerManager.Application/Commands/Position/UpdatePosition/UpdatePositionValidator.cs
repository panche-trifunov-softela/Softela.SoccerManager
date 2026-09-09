using FluentValidation;

namespace SoccerManager.Application.Commands.Position.UpdatePosition;

/// <summary>
/// Validates <see cref="UpdatePositionRequest"/> instances.
/// </summary>
public sealed class UpdatePositionValidator : AbstractValidator<UpdatePositionRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePositionValidator"/> class.
    /// </summary>
    public UpdatePositionValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Area).IsInEnum();
        RuleFor(x => x.Side).IsInEnum();
    }
}
