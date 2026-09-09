using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Position.DeletePosition;

/// <summary>
/// Handles <see cref="DeletePositionRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeletePositionHandler : IRequestHandler<DeletePositionRequest, bool>
{
    private readonly IPositionRepository _positionRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePositionHandler"/> class.
    /// </summary>
    /// <param name="positionRepository">The repository used to load and delete positions.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeletePositionHandler(IPositionRepository positionRepository, IUnitOfWork unitOfWork)
    {
        _positionRepository = positionRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the position identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no position with the given identifier exists.</exception>
    public async Task<bool> Handle(DeletePositionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _positionRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Position {request.Id} not found.");

            await _positionRepository.DeleteAsync(request.Id);

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
