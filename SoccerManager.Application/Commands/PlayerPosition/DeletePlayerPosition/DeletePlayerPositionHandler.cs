using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.PlayerPosition.DeletePlayerPosition;

/// <summary>
/// Handles <see cref="DeletePlayerPositionRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeletePlayerPositionHandler : IRequestHandler<DeletePlayerPositionRequest, bool>
{
    private readonly IPlayerPositionRepository _playerPositionRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePlayerPositionHandler"/> class.
    /// </summary>
    /// <param name="playerPositionRepository">The repository used to load and delete player positions.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeletePlayerPositionHandler(IPlayerPositionRepository playerPositionRepository, IUnitOfWork unitOfWork)
    {
        _playerPositionRepository = playerPositionRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the player position rating identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no player position with the given identifier exists.</exception>
    public async Task<bool> Handle(DeletePlayerPositionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _playerPositionRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"PlayerPosition {request.Id} not found.");

            await _playerPositionRepository.DeleteAsync(request.Id);

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
