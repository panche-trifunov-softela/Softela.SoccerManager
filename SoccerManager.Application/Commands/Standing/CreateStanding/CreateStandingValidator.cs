using FluentValidation;

namespace SoccerManager.Application.Commands.Standing.CreateStanding;

/// <summary>
/// Validates <see cref="CreateStandingRequest"/> instances.
/// </summary>
public sealed class CreateStandingValidator : AbstractValidator<CreateStandingRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateStandingValidator"/> class.
    /// </summary>
    public CreateStandingValidator()
    {
        RuleFor(x => x.CompetitionId).GreaterThan(0);
        RuleFor(x => x.SeasonId).GreaterThan(0);
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
