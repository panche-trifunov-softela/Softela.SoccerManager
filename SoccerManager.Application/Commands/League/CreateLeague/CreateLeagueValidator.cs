using FluentValidation;

namespace SoccerManager.Application.Commands.League.CreateLeague;

/// <summary>
/// Validates <see cref="CreateLeagueRequest"/> instances.
/// </summary>
public sealed class CreateLeagueValidator : AbstractValidator<CreateLeagueRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueValidator"/> class.
    /// </summary>
    public CreateLeagueValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
