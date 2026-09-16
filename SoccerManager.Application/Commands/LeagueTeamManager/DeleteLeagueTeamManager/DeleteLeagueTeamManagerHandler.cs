using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamManager.DeleteLeagueTeamManager;

/// <summary>
/// Handles <see cref="DeleteLeagueTeamManagerRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteLeagueTeamManagerHandler : IRequestHandler<DeleteLeagueTeamManagerRequest, bool>
{
    private readonly ILeagueTeamManagerRepository _leagueTeamManagerRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLeagueTeamManagerHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamManagerRepository">The repository used to load and delete league team managers.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteLeagueTeamManagerHandler(ILeagueTeamManagerRepository leagueTeamManagerRepository, IUnitOfWork unitOfWork)
    {
        _leagueTeamManagerRepository = leagueTeamManagerRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the league team manager appointment identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team manager with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteLeagueTeamManagerRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _leagueTeamManagerRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"LeagueTeamManager {request.Id} not found.");

            await _leagueTeamManagerRepository.DeleteAsync(request.Id);

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
