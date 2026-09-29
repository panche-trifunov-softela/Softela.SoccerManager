using FluentValidation;

namespace SoccerManager.Application.Commands.NationalTeam.UpdateNationalTeam;

/// <summary>
/// Validates <see cref="UpdateNationalTeamRequest"/> instances.
/// </summary>
public sealed class UpdateNationalTeamValidator : AbstractValidator<UpdateNationalTeamRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateNationalTeamValidator"/> class.
    /// </summary>
    public UpdateNationalTeamValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);

        // Shape-only: that the stadium exists is enforced by FK_NationalTeams_Stadiums, and a
        // national team never needs one.
        RuleFor(x => x.StadiumId).GreaterThan(0).When(x => x.StadiumId.HasValue);

        RuleFor(x => x.JerseyUrl).MaximumLength(500);
        RuleFor(x => x.LogoUrl).MaximumLength(500);

        // Shape-only: uniqueness is enforced by UX_NationalTeams_TransfermarktId, and a
        // national team never needs one.
        RuleFor(x => x.TransfermarktId).GreaterThan(0).When(x => x.TransfermarktId.HasValue);
    }
}
