using FluentValidation;

namespace SoccerManager.Application.Commands.MatchTeamStatistic.CreateMatchTeamStatistic;

/// <summary>
/// Validates <see cref="CreateMatchTeamStatisticRequest"/> instances.
/// </summary>
public sealed class CreateMatchTeamStatisticValidator : AbstractValidator<CreateMatchTeamStatisticRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMatchTeamStatisticValidator"/> class.
    /// </summary>
    public CreateMatchTeamStatisticValidator()
    {
        RuleFor(x => x.TeamId).GreaterThan(0);
        RuleFor(x => x.MatchId).GreaterThan(0);

        RuleFor(x => x.ShotsTotal).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ShotsOnTarget).GreaterThanOrEqualTo(0);

        // Shots on target are a subset of all shots, so they can never exceed the total.
        RuleFor(x => x.ShotsOnTarget).LessThanOrEqualTo(x => x.ShotsTotal);

        // Possession is a whole-number percentage; the range matches CK_MatchTeamStatistics_Possession.
        RuleFor(x => x.Possession).InclusiveBetween(0, 100);
    }
}
