using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Commands.LeagueTeamManagerApplication.UpdateLeagueTeamManagerApplication;

/// <summary>
/// Handles <see cref="UpdateLeagueTeamManagerApplicationRequest"/> commands.
/// </summary>
public class UpdateLeagueTeamManagerApplicationHandler : IRequestHandler<UpdateLeagueTeamManagerApplicationRequest, bool>
{
    private readonly ILeagueTeamManagerApplicationRepository _applicationRepository;
    private readonly ILeagueTeamManagerRepository _leagueTeamManagerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueTeamManagerApplicationHandler"/> class.
    /// </summary>
    /// <param name="applicationRepository">The repository used to load and persist league team manager applications.</param>
    /// <param name="leagueTeamManagerRepository">The repository used to persist the appointment an acceptance creates.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the writes into one transaction.</param>
    public UpdateLeagueTeamManagerApplicationHandler(ILeagueTeamManagerApplicationRepository applicationRepository, ILeagueTeamManagerRepository leagueTeamManagerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _applicationRepository = applicationRepository;
        _leagueTeamManagerRepository = leagueTeamManagerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Answers a league team manager application. Accepting it also appoints the manager and rejects the
    /// pending applications the appointment makes moot, all in one transaction.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the answer is recorded.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team manager application with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateLeagueTeamManagerApplicationRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var application = await _applicationRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"LeagueTeamManagerApplication {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateLeagueTeamManagerApplicationMapper.ApplyTo(request, application, now, _currentUser.UserId);

            var answeredId = await _applicationRepository.UpdateAsync(application);

            if (answeredId == 0)
            {
                throw new ValidationException(new[] { new ValidationFailure("Status", "Only a pending application can be answered.") });
            }

            if (application.Status == ApplicationStatus.Accepted)
            {
                var appointment = UpdateLeagueTeamManagerApplicationMapper.ToAppointment(application, now, _currentUser.UserId);

                await _leagueTeamManagerRepository.CreateAsync(appointment);
                await _applicationRepository.RejectOtherPendingAsync(application);
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
