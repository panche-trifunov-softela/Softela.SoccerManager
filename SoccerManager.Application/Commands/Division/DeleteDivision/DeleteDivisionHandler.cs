using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Division.DeleteDivision;

/// <summary>
/// Handles <see cref="DeleteDivisionRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteDivisionHandler : IRequestHandler<DeleteDivisionRequest, bool>
{
    private readonly IDivisionRepository _divisionRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteDivisionHandler"/> class.
    /// </summary>
    /// <param name="divisionRepository">The repository used to load and delete divisions.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteDivisionHandler(IDivisionRepository divisionRepository, IUnitOfWork unitOfWork)
    {
        _divisionRepository = divisionRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the division identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no division with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteDivisionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _divisionRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Division {request.Id} not found.");

            await _divisionRepository.DeleteAsync(request.Id);

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
