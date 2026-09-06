using FluentValidation;

namespace SoccerManager.Application.Commands.Season.UpdateSeason;

/// <summary>
/// Validates <see cref="UpdateSeasonRequest"/> instances.
/// </summary>
public sealed class UpdateSeasonValidator : AbstractValidator<UpdateSeasonRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateSeasonValidator"/> class.
    /// </summary>
    public UpdateSeasonValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.SeasonNumber).GreaterThan(0);
    }
}
