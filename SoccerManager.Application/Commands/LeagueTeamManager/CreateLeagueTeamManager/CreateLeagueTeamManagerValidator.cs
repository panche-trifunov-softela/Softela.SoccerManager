using FluentValidation;

namespace SoccerManager.Application.Commands.LeagueTeamManager.CreateLeagueTeamManager;

/// <summary>
/// Validates <see cref="CreateLeagueTeamManagerRequest"/> instances.
/// </summary>
public sealed class CreateLeagueTeamManagerValidator : AbstractValidator<CreateLeagueTeamManagerRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamManagerValidator"/> class.
    /// </summary>
    public CreateLeagueTeamManagerValidator()
    {
        RuleFor(x => x.LeagueId).GreaterThan(0);
        RuleFor(x => x.TeamId).GreaterThan(0);
        RuleFor(x => x.ManagerId).GreaterThan(0);

        // A missing startDate binds to DateOnly's default of 0001-01-01, so the lower bound
        // rejects an omitted value as well as a nonsensical one.
        RuleFor(x => x.StartDate).GreaterThan(new DateOnly(1900, 1, 1));

        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate.HasValue);

        RuleFor(x => x.EndDate).Null().When(x => x.IsCurrent).WithMessage("EndDate must be null while the appointment is current.");
    }
}
