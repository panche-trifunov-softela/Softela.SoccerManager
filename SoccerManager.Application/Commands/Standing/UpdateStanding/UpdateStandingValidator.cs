using FluentValidation;

namespace SoccerManager.Application.Commands.Standing.UpdateStanding;

/// <summary>
/// Validates <see cref="UpdateStandingRequest"/> instances.
/// </summary>
public sealed class UpdateStandingValidator : AbstractValidator<UpdateStandingRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateStandingValidator"/> class.
    /// </summary>
    public UpdateStandingValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.SeasonId).GreaterThan(0);
        RuleFor(x => x.DivisionId).GreaterThan(0);
        RuleFor(x => x.TeamId).GreaterThan(0);

        // Unlike the identifier fields above, 0 is a legitimate value for a statistic.
        RuleFor(x => x.Points).GreaterThanOrEqualTo(0);
        RuleFor(x => x.GoalsFor).GreaterThanOrEqualTo(0);
        RuleFor(x => x.GoalsAgainst).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Wins).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Draws).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Losses).GreaterThanOrEqualTo(0);
    }
}
