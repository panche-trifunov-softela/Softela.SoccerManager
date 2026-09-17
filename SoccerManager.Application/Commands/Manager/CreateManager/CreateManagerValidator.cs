using FluentValidation;

namespace SoccerManager.Application.Commands.Manager.CreateManager;

/// <summary>
/// Validates <see cref="CreateManagerRequest"/> instances.
/// </summary>
public sealed class CreateManagerValidator : AbstractValidator<CreateManagerRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateManagerValidator"/> class.
    /// </summary>
    public CreateManagerValidator()
    {
        RuleFor(x => x.ImageUrl).MaximumLength(500);
    }
}
