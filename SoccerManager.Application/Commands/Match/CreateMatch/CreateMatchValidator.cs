using FluentValidation;

namespace SoccerManager.Application.Commands.Match.CreateMatch;

/// <summary>
/// Validates <see cref="CreateMatchRequest"/> instances.
/// </summary>
public sealed class CreateMatchValidator : AbstractValidator<CreateMatchRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMatchValidator"/> class.
    /// </summary>
    public CreateMatchValidator()
    {
        RuleFor(x => x.SeasonId).GreaterThan(0);
        RuleFor(x => x.DivisionId).GreaterThan(0);
        RuleFor(x => x.HomeTeamId).GreaterThan(0);
        RuleFor(x => x.AwayTeamId).GreaterThan(0);
        RuleFor(x => x.RefereeId).GreaterThan(0);

        RuleFor(x => x.AwayTeamId).NotEqual(x => x.HomeTeamId);
    }
}
