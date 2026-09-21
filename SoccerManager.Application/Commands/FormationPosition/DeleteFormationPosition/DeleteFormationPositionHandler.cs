using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.FormationPosition.DeleteFormationPosition;

/// <summary>
/// Handles <see cref="DeleteFormationPositionRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteFormationPositionHandler : IRequestHandler<DeleteFormationPositionRequest, bool>
{
    private readonly IFormationPositionRepository _formationPositionRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteFormationPositionHandler"/> class.
    /// </summary>
    /// <param name="formationPositionRepository">The repository used to load and delete formation positions.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteFormationPositionHandler(IFormationPositionRepository formationPositionRepository, IUnitOfWork unitOfWork)
    {
        _formationPositionRepository = formationPositionRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the formation position slot identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no formation position with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteFormationPositionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _formationPositionRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Formation position {request.Id} not found.");

            await _formationPositionRepository.DeleteAsync(request.Id);

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
