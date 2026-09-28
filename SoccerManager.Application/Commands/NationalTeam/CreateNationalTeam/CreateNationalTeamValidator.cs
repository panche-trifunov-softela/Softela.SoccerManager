using FluentValidation;

namespace SoccerManager.Application.Commands.NationalTeam.CreateNationalTeam;

/// <summary>
/// Validates <see cref="CreateNationalTeamRequest"/> instances.
/// </summary>
public sealed class CreateNationalTeamValidator : AbstractValidator<CreateNationalTeamRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateNationalTeamValidator"/> class.
    /// </summary>
    public CreateNationalTeamValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);

        // Shape-only: that the stadium exists is enforced by FK_NationalTeams_Stadiums, and a
        // national team never needs one.
        RuleFor(x => x.StadiumId).GreaterThan(0).When(x => x.StadiumId.HasValue);

        RuleFor(x => x.JerseyUrl).MaximumLength(500);
        RuleFor(x => x.LogoUrl).MaximumLength(500);
    }
}
