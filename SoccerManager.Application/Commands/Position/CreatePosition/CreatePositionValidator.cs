using FluentValidation;

namespace SoccerManager.Application.Commands.Position.CreatePosition;

/// <summary>
/// Validates <see cref="CreatePositionRequest"/> instances.
/// </summary>
public sealed class CreatePositionValidator : AbstractValidator<CreatePositionRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePositionValidator"/> class.
    /// </summary>
    public CreatePositionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Area).IsInEnum();
        RuleFor(x => x.Side).IsInEnum();
    }
}
