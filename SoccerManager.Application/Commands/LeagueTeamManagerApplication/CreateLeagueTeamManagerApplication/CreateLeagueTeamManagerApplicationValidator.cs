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
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamManagerApplicationValidator"/> class.
    /// </summary>
    /// <param name="managerRepository">The repository used to check the caller has a manager profile.</param>
    /// <param name="leagueTeamManagerRepository">The repository used to check the team has no manager and the caller manages no club in the league.</param>
    /// <param name="leagueTeamRepository">The repository used to find the club's roster entry and its league.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    public CreateLeagueTeamManagerApplicationValidator(IManagerRepository managerRepository, ILeagueTeamManagerRepository leagueTeamManagerRepository, ILeagueTeamRepository leagueTeamRepository, ICurrentUser currentUser)
    {
        _managerRepository = managerRepository;
        _leagueTeamManagerRepository = leagueTeamManagerRepository;
        _leagueTeamRepository = leagueTeamRepository;
        _currentUser = currentUser;

        RuleFor(x => x.LeagueTeamId).GreaterThan(0);

        // These lookups run outside the handler's transaction, so a race can at worst let a pending
        // application through; answering it re-checks the team and applicant rules. The manager's
        // existence is also enforced by FK_LeagueTeamManagerApplications_Managers.
        When(x => x.LeagueTeamId > 0, () =>
        {
            RuleFor(x => x)
                .MustAsync(HaveManagerProfileAsync)
                .OverridePropertyName("Manager")
                .WithMessage("Create a manager profile first.");

            RuleFor(x => x.LeagueTeamId).CustomAsync(ValidateClubAsync);
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
    /// Checks that the club is on a league's roster, that it has no current manager there and that the
    /// caller manages no club in that league, loading the roster entry once for every check.
    /// </summary>
    /// <param name="leagueTeamId">The roster entry the caller applies to manage.</param>
    /// <param name="context">The context failures are added to.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    private async Task ValidateClubAsync(int leagueTeamId, ValidationContext<CreateLeagueTeamManagerApplicationRequest> context, CancellationToken cancellationToken)
    {
        // The roster is what matters, not a squad: a league may start its managers with no players. An
        // unknown roster entry is reported on its own.
        var leagueTeam = await _leagueTeamRepository.GetByIdAsync(leagueTeamId);

        if (leagueTeam is null)
        {
            context.AddFailure("The team is not part of this league.");

            return;
        }

        var teamAppointments = await _leagueTeamManagerRepository.GetByLeagueTeamIdAsync(leagueTeamId);

        if (teamAppointments.Any(appointment => appointment.IsCurrent))
        {
            context.AddFailure("The team already has a manager in this league.");
        }

        var callerAppointments = await _leagueTeamManagerRepository.GetByUserIdAsync(_currentUser.UserId, currentOnly: true);

        if (callerAppointments.Any(appointment => appointment.LeagueId == leagueTeam.LeagueId))
        {
            context.AddFailure("You already manage a club in this league.");
        }
    }
}
