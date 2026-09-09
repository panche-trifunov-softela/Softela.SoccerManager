using FluentValidation;

namespace SoccerManager.Application.Commands.Division.CreateDivision;

/// <summary>
/// Validates <see cref="CreateDivisionRequest"/> instances.
/// </summary>
public sealed class CreateDivisionValidator : AbstractValidator<CreateDivisionRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateDivisionValidator"/> class.
    /// </summary>
    public CreateDivisionValidator()
    {
        RuleFor(x => x.LeagueId).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Order).GreaterThan(0);

        // Zero is legitimate for all three counts: the top division promotes nobody
        // and the bottom division relegates nobody.
        RuleFor(x => x.TeamsPromoted).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TeamsRelegated).GreaterThanOrEqualTo(0);
        RuleFor(x => x.TeamsInPlayoffs).GreaterThanOrEqualTo(0);
    }
}
