using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamManagerApplication.DeleteLeagueTeamManagerApplication;

/// <summary>
/// Handles <see cref="DeleteLeagueTeamManagerApplicationRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag, and it leaves any appointment an acceptance created untouched.
/// </summary>
public class DeleteLeagueTeamManagerApplicationHandler : IRequestHandler<DeleteLeagueTeamManagerApplicationRequest, bool>
{
    private readonly ILeagueTeamManagerApplicationRepository _applicationRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLeagueTeamManagerApplicationHandler"/> class.
    /// </summary>
    /// <param name="applicationRepository">The repository used to load and delete league team manager applications.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteLeagueTeamManagerApplicationHandler(ILeagueTeamManagerApplicationRepository applicationRepository, IUnitOfWork unitOfWork)
    {
        _applicationRepository = applicationRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the league team manager application identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team manager application with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteLeagueTeamManagerApplicationRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _applicationRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"LeagueTeamManagerApplication {request.Id} not found.");

            await _applicationRepository.DeleteAsync(request.Id);

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
