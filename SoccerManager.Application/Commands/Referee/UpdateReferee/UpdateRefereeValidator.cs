using FluentValidation;

namespace SoccerManager.Application.Commands.Referee.UpdateReferee;

/// <summary>
/// Validates <see cref="UpdateRefereeRequest"/> instances.
/// </summary>
public sealed class UpdateRefereeValidator : AbstractValidator<UpdateRefereeRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateRefereeValidator"/> class.
    /// </summary>
    public UpdateRefereeValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);

        RuleFor(x => x.ImageUrl).MaximumLength(500);

        // An omitted tolerance is already Balanced, so this only rejects an explicit value outside
        // the enum, zero included.
        RuleFor(x => x.Tolerance).IsInEnum();
    }
}
