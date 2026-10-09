using FluentValidation;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamManagerApplication.CreateLeagueTeamManagerApplication;

/// <summary>
/// Validates <see cref="CreateLeagueTeamManagerApplicationRequest"/> instances.
/// </summary>
public sealed class CreateLeagueTeamManagerApplicationValidator : AbstractValidator<CreateLeagueTeamManagerApplicationRequest>
{
    private readonly IManagerRepository _managerRepository;
    private readonly ILeagueTeamManagerRepository _leagueTeamManagerRepository;
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamManagerApplicationValidator"/> class.
    /// </summary>
    /// <param name="managerRepository">The repository used to check the caller has a manager profile.</param>
    /// <param name="leagueTeamManagerRepository">The repository used to check the team has no manager and the caller manages no club in the league.</param>
    /// <param name="leagueTeamPlayerRepository">The repository used to check the team has a squad in the league.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    public CreateLeagueTeamManagerApplicationValidator(IManagerRepository managerRepository, ILeagueTeamManagerRepository leagueTeamManagerRepository, ILeagueTeamPlayerRepository leagueTeamPlayerRepository, ICurrentUser currentUser)
    {
        _managerRepository = managerRepository;
        _leagueTeamManagerRepository = leagueTeamManagerRepository;
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        _currentUser = currentUser;

        RuleFor(x => x.LeagueId).GreaterThan(0);
        RuleFor(x => x.TeamId).GreaterThan(0);

        // These lookups run outside the handler's transaction, so a race can at worst let a pending
        // application through; answering it re-checks the team and applicant rules. The manager's
        // existence is also enforced by FK_LeagueTeamManagerApplications_Managers.
        When(x => x.LeagueId > 0 && x.TeamId > 0, () =>
        {
            RuleFor(x => x)
                .MustAsync(HaveManagerProfileAsync)
                .OverridePropertyName("Manager")
                .WithMessage("Create a manager profile first.");

            RuleFor(x => x.TeamId)
                .MustAsync((request, _, _) => TeamHasSquadAsync(request))
                .WithMessage("The team has no squad in this league.");

            RuleFor(x => x.TeamId)
                .MustAsync((request, _, _) => TeamHasNoManagerAsync(request))
                .WithMessage("The team already has a manager in this league.");

            RuleFor(x => x.LeagueId)
                .MustAsync((request, _, _) => CallerManagesNoClubAsync(request))
                .WithMessage("You already manage a club in this league.");
        });
    }

    /// <summary>
    /// Checks that the caller has a manager profile.
    /// </summary>
    /// <param name="request">The request being validated.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns><see langword="true"/> when the caller has a manager profile.</returns>
    private async Task<bool> HaveManagerProfileAsync(CreateLeagueTeamManagerApplicationRequest request, CancellationToken cancellationToken)
    {
        // A caller who is not yet a manager is told to create a profile; one is never created on their behalf.
        return await _managerRepository.GetByUserIdAsync(_currentUser.UserId) is not null;
    }

    /// <summary>
    /// Checks that the team has at least one player registered in the league.
    /// </summary>
    /// <param name="request">The request being validated.</param>
    /// <returns><see langword="true"/> when the team has a squad in the league.</returns>
    private async Task<bool> TeamHasSquadAsync(CreateLeagueTeamManagerApplicationRequest request)
    {
        var squad = await _leagueTeamPlayerRepository.GetByLeagueAndTeamAsync(request.LeagueId, request.TeamId);

        return squad.Count > 0;
    }

    /// <summary>
    /// Checks that the team has no current manager in the league.
    /// </summary>
    /// <param name="request">The request being validated.</param>
    /// <returns><see langword="true"/> when no current manager is appointed to the team in the league.</returns>
    private async Task<bool> TeamHasNoManagerAsync(CreateLeagueTeamManagerApplicationRequest request)
    {
        var appointments = await _leagueTeamManagerRepository.GetByLeagueAndTeamAsync(request.LeagueId, request.TeamId);

        return !appointments.Any(appointment => appointment.IsCurrent);
    }

    /// <summary>
    /// Checks that the caller does not currently manage any club in the league.
    /// </summary>
    /// <param name="request">The request being validated.</param>
    /// <returns><see langword="true"/> when the caller has no current appointment in the league.</returns>
    private async Task<bool> CallerManagesNoClubAsync(CreateLeagueTeamManagerApplicationRequest request)
    {
        var appointments = await _leagueTeamManagerRepository.GetByUserIdAsync(_currentUser.UserId, currentOnly: true);

        return !appointments.Any(appointment => appointment.LeagueId == request.LeagueId);
    }
}
