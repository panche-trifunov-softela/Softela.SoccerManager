using FluentValidation;

namespace SoccerManager.Application.Commands.Team.CreateTeam;

/// <summary>
/// Validates <see cref="CreateTeamRequest"/> instances.
/// </summary>
public sealed class CreateTeamValidator : AbstractValidator<CreateTeamRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateTeamValidator"/> class.
    /// </summary>
    public CreateTeamValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.FinancialState).IsInEnum();

        // Stadiums are not implemented yet, so this only sanity-checks a supplied value
        // and never requires one.
        RuleFor(x => x.StadiumId).GreaterThan(0).When(x => x.StadiumId.HasValue);

        RuleFor(x => x.JerseyUrl).MaximumLength(500);
    }
}
