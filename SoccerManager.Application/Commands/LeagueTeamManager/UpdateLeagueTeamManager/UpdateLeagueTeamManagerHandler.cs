using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamManager.UpdateLeagueTeamManager;

/// <summary>
/// Handles <see cref="UpdateLeagueTeamManagerRequest"/> commands.
/// </summary>
public class UpdateLeagueTeamManagerHandler : IRequestHandler<UpdateLeagueTeamManagerRequest, bool>
{
    private readonly ILeagueTeamManagerRepository _leagueTeamManagerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueTeamManagerHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamManagerRepository">The repository used to load and persist league team managers.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateLeagueTeamManagerHandler(ILeagueTeamManagerRepository leagueTeamManagerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamManagerRepository = leagueTeamManagerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing league team manager appointment with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team manager with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateLeagueTeamManagerRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var leagueTeamManager = await _leagueTeamManagerRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"LeagueTeamManager {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateLeagueTeamManagerMapper.ApplyTo(request, leagueTeamManager, now, _currentUser.UserId);

            await _leagueTeamManagerRepository.UpdateAsync(leagueTeamManager);

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
