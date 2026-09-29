using FluentValidation;

namespace SoccerManager.Application.Commands.Referee.CreateReferee;

/// <summary>
/// Validates <see cref="CreateRefereeRequest"/> instances.
/// </summary>
public sealed class CreateRefereeValidator : AbstractValidator<CreateRefereeRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRefereeValidator"/> class.
    /// </summary>
    public CreateRefereeValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);

        RuleFor(x => x.ImageUrl).MaximumLength(500);

        // An omitted tolerance is already Balanced, so this only rejects an explicit value outside
        // the enum, zero included.
        RuleFor(x => x.Tolerance).IsInEnum();
    }
}
