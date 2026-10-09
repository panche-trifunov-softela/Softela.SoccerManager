using FluentValidation;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.LeagueTeamManagerApplication.UpdateLeagueTeamManagerApplication;

/// <summary>
/// Validates <see cref="UpdateLeagueTeamManagerApplicationRequest"/> instances.
/// </summary>
public sealed class UpdateLeagueTeamManagerApplicationValidator : AbstractValidator<UpdateLeagueTeamManagerApplicationRequest>
{
    private readonly ILeagueTeamManagerApplicationRepository _applicationRepository;
    private readonly ILeagueTeamManagerRepository _leagueTeamManagerRepository;
    private readonly IManagerRepository _managerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueTeamManagerApplicationValidator"/> class.
    /// </summary>
    /// <param name="applicationRepository">The repository used to load the application being answered.</param>
    /// <param name="leagueTeamManagerRepository">The repository used to check the team and the applicant are still free to be appointed.</param>
    /// <param name="managerRepository">The repository used to find the applicant's user.</param>
    public UpdateLeagueTeamManagerApplicationValidator(ILeagueTeamManagerApplicationRepository applicationRepository, ILeagueTeamManagerRepository leagueTeamManagerRepository, IManagerRepository managerRepository)
    {
        _applicationRepository = applicationRepository;
        _leagueTeamManagerRepository = leagueTeamManagerRepository;
        _managerRepository = managerRepository;

        RuleFor(x => x.Id).GreaterThan(0);

        // A zero fails IsInEnum, and Pending is not an answer.
        RuleFor(x => x.Status)
            .IsInEnum()
            .NotEqual(ApplicationStatus.Pending).WithMessage("An application can only be answered with Accepted or Rejected.");

        // The database is only queried for a request that already carries a valid answer. A missing
        // application adds no failure here, so the handler can answer it with a 404.
        When(x => x.Id > 0 && x.Status is ApplicationStatus.Accepted or ApplicationStatus.Rejected, () =>
        {
            RuleFor(x => x.Status).CustomAsync(ValidateAnswerAsync);
        });
    }

    /// <summary>
    /// Checks that the application can still be answered as requested, loading it once for every check.
    /// </summary>
    /// <param name="status">The answer being given.</param>
    /// <param name="context">The context failures are added to.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    private async Task ValidateAnswerAsync(ApplicationStatus status, ValidationContext<UpdateLeagueTeamManagerApplicationRequest> context, CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.GetByIdAsync(context.InstanceToValidate.Id);

        if (application is null)
        {
            return;
        }

        if (application.Status != ApplicationStatus.Pending)
        {
            context.AddFailure("Only a pending application can be answered.");

            return;
        }

        if (status != ApplicationStatus.Accepted)
        {
            return;
        }

        // Pending is also enforced atomically by the update procedure, and the team check by
        // UX_LeagueTeamManagers_LeagueId_TeamId_Current. The applicant check has no database backstop:
        // two concurrent acceptances of one applicant for different teams could both pass.
        var teamAppointments = await _leagueTeamManagerRepository.GetByLeagueAndTeamAsync(application.LeagueId, application.TeamId);

        if (teamAppointments.Any(appointment => appointment.IsCurrent))
        {
            context.AddFailure("The team already has a manager in this league.");
        }

        var applicant = await _managerRepository.GetByIdAsync(application.ManagerId);

        // The Managers cascade deletes the application with the manager, so a missing row cannot happen.
        if (applicant is null)
        {
            return;
        }

        var applicantAppointments = await _leagueTeamManagerRepository.GetByUserIdAsync(applicant.UserId, currentOnly: true);

        if (applicantAppointments.Any(appointment => appointment.LeagueId == application.LeagueId))
        {
            context.AddFailure("The applicant already manages a club in this league.");
        }
    }
}
