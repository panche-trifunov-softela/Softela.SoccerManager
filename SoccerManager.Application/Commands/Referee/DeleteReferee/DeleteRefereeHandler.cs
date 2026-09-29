using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Referee.DeleteReferee;

/// <summary>
/// Handles <see cref="DeleteRefereeRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteRefereeHandler : IRequestHandler<DeleteRefereeRequest, bool>
{
    private readonly IRefereeRepository _refereeRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteRefereeHandler"/> class.
    /// </summary>
    /// <param name="refereeRepository">The repository used to load and delete referees.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteRefereeHandler(IRefereeRepository refereeRepository, IUnitOfWork unitOfWork)
    {
        _refereeRepository = refereeRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the referee identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no referee with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteRefereeRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _refereeRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Referee {request.Id} not found.");

            await _refereeRepository.DeleteAsync(request.Id);

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
