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
        RuleFor(x => x.DivisionId).GreaterThan(0);
        RuleFor(x => x.RefereeId).GreaterThan(0);

        RuleFor(x => x.Attendance).GreaterThanOrEqualTo(0);
    }
}
