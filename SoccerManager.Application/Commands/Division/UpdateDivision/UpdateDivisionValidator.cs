using FluentValidation;

namespace SoccerManager.Application.Commands.Division.UpdateDivision;

/// <summary>
/// Validates <see cref="UpdateDivisionRequest"/> instances.
/// </summary>
public sealed class UpdateDivisionValidator : AbstractValidator<UpdateDivisionRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateDivisionValidator"/> class.
    /// </summary>
    public UpdateDivisionValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Order).GreaterThan(0);

        // Zero is legitimate for all three counts: the top division promotes nobody
        // and the bottom division relegates nobody.
        RuleFor(x => x.TeamsPromoted).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TeamsRelegated).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TeamsInPlayoffs).GreaterThanOrEqualTo(0);
    }
}
