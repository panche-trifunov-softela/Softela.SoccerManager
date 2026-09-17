using FluentValidation;

namespace SoccerManager.Application.Commands.Stadium.UpdateStadium;

/// <summary>
/// Validates <see cref="UpdateStadiumRequest"/> instances.
/// </summary>
public sealed class UpdateStadiumValidator : AbstractValidator<UpdateStadiumRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateStadiumValidator"/> class.
    /// </summary>
    public UpdateStadiumValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ImageUrl).MaximumLength(500);
        RuleFor(x => x.Size).GreaterThan(0);
    }
}
