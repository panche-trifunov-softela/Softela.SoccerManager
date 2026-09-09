using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Team.DeleteTeam;

/// <summary>
/// Handles <see cref="DeleteTeamRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteTeamHandler : IRequestHandler<DeleteTeamRequest, bool>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteTeamHandler"/> class.
    /// </summary>
    /// <param name="teamRepository">The repository used to load and delete teams.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteTeamHandler(ITeamRepository teamRepository, IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the team identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no team with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteTeamRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _teamRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Team {request.Id} not found.");

            await _teamRepository.DeleteAsync(request.Id);

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
