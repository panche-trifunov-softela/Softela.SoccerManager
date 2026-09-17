using FluentValidation;

namespace SoccerManager.Application.Commands.Stadium.CreateStadium;

/// <summary>
/// Validates <see cref="CreateStadiumRequest"/> instances.
/// </summary>
public sealed class CreateStadiumValidator : AbstractValidator<CreateStadiumRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateStadiumValidator"/> class.
    /// </summary>
    public CreateStadiumValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ImageUrl).MaximumLength(500);
        RuleFor(x => x.Size).GreaterThan(0);
    }
}
