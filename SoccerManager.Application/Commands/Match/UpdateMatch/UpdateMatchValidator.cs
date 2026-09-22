using FluentValidation;

namespace SoccerManager.Application.Commands.Match.UpdateMatch;

/// <summary>
/// Validates <see cref="UpdateMatchRequest"/> instances.
/// </summary>
public sealed class UpdateMatchValidator : AbstractValidator<UpdateMatchRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMatchValidator"/> class.
    /// </summary>
    public UpdateMatchValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.SeasonId).GreaterThan(0);
        // A friendly belongs to no competition, so the shape rule only applies when one was supplied.
        RuleFor(x => x.CompetitionId).GreaterThan(0).When(x => x.CompetitionId.HasValue);
        RuleFor(x => x.RefereeId).GreaterThan(0);

        RuleFor(x => x.Attendance).GreaterThanOrEqualTo(0);
    }
}
