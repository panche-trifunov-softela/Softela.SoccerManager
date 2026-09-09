using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Player.DeletePlayer;

/// <summary>
/// Handles <see cref="DeletePlayerRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeletePlayerHandler : IRequestHandler<DeletePlayerRequest, bool>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePlayerHandler"/> class.
    /// </summary>
    /// <param name="playerRepository">The repository used to load and delete players.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeletePlayerHandler(IPlayerRepository playerRepository, IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the player identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no player with the given identifier exists.</exception>
    public async Task<bool> Handle(DeletePlayerRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _playerRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Player {request.Id} not found.");

            await _playerRepository.DeleteAsync(request.Id);

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
