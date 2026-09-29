using FluentValidation;

namespace SoccerManager.Application.Commands.Team.UpdateTeam;

/// <summary>
/// Validates <see cref="UpdateTeamRequest"/> instances.
/// </summary>
public sealed class UpdateTeamValidator : AbstractValidator<UpdateTeamRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTeamValidator"/> class.
    /// </summary>
    public UpdateTeamValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
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
