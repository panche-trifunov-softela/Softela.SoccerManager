using FluentValidation;

namespace SoccerManager.Application.Commands.League.UpdateLeague;

/// <summary>
/// Validates <see cref="UpdateLeagueRequest"/> instances.
/// </summary>
public sealed class UpdateLeagueValidator : AbstractValidator<UpdateLeagueRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueValidator"/> class.
    /// </summary>
    public UpdateLeagueValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
