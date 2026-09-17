using FluentValidation;

namespace SoccerManager.Application.Commands.Manager.UpdateManager;

/// <summary>
/// Validates <see cref="UpdateManagerRequest"/> instances.
/// </summary>
public sealed class UpdateManagerValidator : AbstractValidator<UpdateManagerRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateManagerValidator"/> class.
    /// </summary>
    public UpdateManagerValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.ImageUrl).MaximumLength(500);
    }
}
