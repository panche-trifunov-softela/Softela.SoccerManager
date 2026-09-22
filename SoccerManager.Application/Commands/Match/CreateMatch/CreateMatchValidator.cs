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
        // A friendly belongs to no competition, so the shape rule only applies when one was supplied.
        RuleFor(x => x.CompetitionId).GreaterThan(0).When(x => x.CompetitionId.HasValue);
        RuleFor(x => x.RefereeId).GreaterThan(0);

        RuleFor(x => x.Attendance).GreaterThanOrEqualTo(0);
    }
}
