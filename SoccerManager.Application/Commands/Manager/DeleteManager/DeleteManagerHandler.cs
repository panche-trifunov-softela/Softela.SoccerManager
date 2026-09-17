using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Manager.DeleteManager;

/// <summary>
/// Handles <see cref="DeleteManagerRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteManagerHandler : IRequestHandler<DeleteManagerRequest, bool>
{
    private readonly IManagerRepository _managerRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteManagerHandler"/> class.
    /// </summary>
    /// <param name="managerRepository">The repository used to load and delete managers.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteManagerHandler(IManagerRepository managerRepository, IUnitOfWork unitOfWork)
    {
        _managerRepository = managerRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the manager profile identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no manager with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteManagerRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _managerRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Manager {request.Id} not found.");

            await _managerRepository.DeleteAsync(request.Id);

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
