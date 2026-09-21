using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Formation.DeleteFormation;

/// <summary>
/// Handles <see cref="DeleteFormationRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteFormationHandler : IRequestHandler<DeleteFormationRequest, bool>
{
    private readonly IFormationRepository _formationRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteFormationHandler"/> class.
    /// </summary>
    /// <param name="formationRepository">The repository used to load and delete formations.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteFormationHandler(IFormationRepository formationRepository, IUnitOfWork unitOfWork)
    {
        _formationRepository = formationRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the formation identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no formation with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteFormationRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _formationRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Formation {request.Id} not found.");

            await _formationRepository.DeleteAsync(request.Id);

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
