using FluentValidation;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeam.CreateLeagueTeam;

/// <summary>
/// Validates <see cref="CreateLeagueTeamRequest"/> instances.
/// </summary>
public sealed class CreateLeagueTeamValidator : AbstractValidator<CreateLeagueTeamRequest>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamValidator"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to check the team is not already on the league's roster.</param>
    public CreateLeagueTeamValidator(ILeagueTeamRepository leagueTeamRepository)
    {
        _leagueTeamRepository = leagueTeamRepository;

        RuleFor(x => x.LeagueId).GreaterThan(0);
        RuleFor(x => x.TeamId).GreaterThan(0);

        // The roster is only looked up once both ids are usable; an unknown league or team is
        // left to the foreign keys, as in the sibling slices.
        When(x => x.LeagueId > 0 && x.TeamId > 0, () =>
        {
            RuleFor(x => x.TeamId)
                .MustAsync((request, _, _) => NotAlreadyOnRosterAsync(request))
                .WithMessage("The team is already in this league.");
        });
    }

    /// <summary>
    /// Checks that the team is not already on the roster of the league.
    /// </summary>
    /// <param name="request">The request being validated.</param>
    /// <returns><see langword="true"/> when the team is not yet part of the league.</returns>
    private async Task<bool> NotAlreadyOnRosterAsync(CreateLeagueTeamRequest request)
    {
        // UQ_LeagueTeams_LeagueId_TeamId is the real guarantee; this only turns a duplicate into a 400 instead of a failed insert.
        return await _leagueTeamRepository.GetByLeagueAndTeamAsync(request.LeagueId, request.TeamId) is null;
    }
}
