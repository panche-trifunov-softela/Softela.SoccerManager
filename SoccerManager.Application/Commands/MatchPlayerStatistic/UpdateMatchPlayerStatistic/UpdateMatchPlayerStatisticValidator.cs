using FluentValidation;

namespace SoccerManager.Application.Commands.MatchPlayerStatistic.UpdateMatchPlayerStatistic;

/// <summary>
/// Validates <see cref="UpdateMatchPlayerStatisticRequest"/> instances.
/// </summary>
public sealed class UpdateMatchPlayerStatisticValidator : AbstractValidator<UpdateMatchPlayerStatisticRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMatchPlayerStatisticValidator"/> class.
    /// </summary>
    public UpdateMatchPlayerStatisticValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.PlayerId).GreaterThan(0);
        RuleFor(x => x.MatchId).GreaterThan(0);

        // PrecisionScale matches the DECIMAL(3,1) column, so an over-precise value is a 400
        // rather than a silent rounding at the database.
        RuleFor(x => x.Rating).InclusiveBetween(5.0m, 10.0m).PrecisionScale(3, 1, false);

        RuleFor(x => x.MinutesPlayed).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Goals).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Assists).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PenaltiesScored).GreaterThanOrEqualTo(0);
        RuleFor(x => x.PenaltiesMissed).GreaterThanOrEqualTo(0);
        RuleFor(x => x.YellowCards).GreaterThanOrEqualTo(0);
        RuleFor(x => x.RedCards).GreaterThanOrEqualTo(0);

        // A scored penalty is a goal, so the total can never be lower than the penalties in it.
        RuleFor(x => x.Goals).GreaterThanOrEqualTo(x => x.PenaltiesScored);
    }
}
