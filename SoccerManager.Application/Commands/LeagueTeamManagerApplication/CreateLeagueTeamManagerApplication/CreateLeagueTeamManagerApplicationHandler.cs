using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamManagerApplication.CreateLeagueTeamManagerApplication;

/// <summary>
/// Handles <see cref="CreateLeagueTeamManagerApplicationRequest"/> commands.
/// </summary>
public class CreateLeagueTeamManagerApplicationHandler : IRequestHandler<CreateLeagueTeamManagerApplicationRequest, int>
{
    private readonly ILeagueTeamManagerApplicationRepository _applicationRepository;
    private readonly IManagerRepository _managerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamManagerApplicationHandler"/> class.
    /// </summary>
    /// <param name="applicationRepository">The repository used to persist league team manager applications.</param>
    /// <param name="managerRepository">The repository used to find the caller's manager profile.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateLeagueTeamManagerApplicationHandler(ILeagueTeamManagerApplicationRepository applicationRepository, IManagerRepository managerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _applicationRepository = applicationRepository;
        _managerRepository = managerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new pending league team manager application for the authenticated caller.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created application.</returns>
    public async Task<int> Handle(CreateLeagueTeamManagerApplicationRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        // The validator has already checked the caller has a manager profile.
        var manager = await _managerRepository.GetByUserIdAsync(_currentUser.UserId);

        var application = CreateLeagueTeamManagerApplicationMapper.ToDomainEntity(request, manager!.Id, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _applicationRepository.CreateAsync(application);

            await _unitOfWork.CommitAsync(cancellationToken);

            return id;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
