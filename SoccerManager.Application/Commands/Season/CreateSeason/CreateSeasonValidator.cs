using FluentValidation;

namespace SoccerManager.Application.Commands.Season.CreateSeason;

/// <summary>
/// Validates <see cref="CreateSeasonRequest"/> instances.
/// </summary>
public sealed class CreateSeasonValidator : AbstractValidator<CreateSeasonRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateSeasonValidator"/> class.
    /// </summary>
    public CreateSeasonValidator()
    {
        RuleFor(x => x.LeagueId).GreaterThan(0);
        RuleFor(x => x.SeasonNumber).GreaterThan(0);
    }
}
