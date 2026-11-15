using FluentValidation;
using SoccerManager.Application.Core.Extensions;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Standing.UpdateStanding;

/// <summary>
/// Validates <see cref="UpdateStandingRequest"/> instances.
/// </summary>
public sealed class UpdateStandingValidator : AbstractValidator<UpdateStandingRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateStandingValidator"/> class.
    /// </summary>
    /// <param name="competitionRepository">The repository used to find the competition's order.</param>
    /// <param name="leagueTeamRepository">The repository used to check whether the club is a guest.</param>
    public UpdateStandingValidator(ICompetitionRepository competitionRepository, ILeagueTeamRepository leagueTeamRepository)
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.CompetitionId).GreaterThan(0);
        RuleFor(x => x.SeasonId).GreaterThan(0);
        RuleFor(x => x.LeagueTeamId).GreaterThan(0);

        // The lookups run outside the handler's transaction; the guard in dbo.UpdateStanding is the backstop. A guest may
        // have a cup row (Order 0), never a division row.
        When(x => x.CompetitionId > 0 && x.LeagueTeamId > 0, () =>
        {
            RuleFor(x => x).MustNotBePlacedInDivision(x => x.CompetitionId, x => x.LeagueTeamId, competitionRepository, leagueTeamRepository);
        });

        // Unlike the identifier fields above, 0 is a legitimate value for a statistic.
        RuleFor(x => x.Points).GreaterThanOrEqualTo(0);
        RuleFor(x => x.GoalsFor).GreaterThanOrEqualTo(0);
        RuleFor(x => x.GoalsAgainst).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Wins).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Draws).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Losses).GreaterThanOrEqualTo(0);
    }
}
