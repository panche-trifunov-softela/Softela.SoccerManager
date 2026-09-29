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

        // Shape-only: that the stadium exists is enforced by FK_Teams_Stadiums, and a team
        // never needs one.
        RuleFor(x => x.StadiumId).GreaterThan(0).When(x => x.StadiumId.HasValue);

        RuleFor(x => x.JerseyUrl).MaximumLength(500);
        RuleFor(x => x.LogoUrl).MaximumLength(500);

        // Shape-only: uniqueness is enforced by UX_Teams_TransfermarktId, and a team
        // never needs one.
        RuleFor(x => x.TransfermarktId).GreaterThan(0).When(x => x.TransfermarktId.HasValue);
    }
}
