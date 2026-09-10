using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Standing.DeleteStanding;

/// <summary>
/// Handles <see cref="DeleteStandingRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteStandingHandler : IRequestHandler<DeleteStandingRequest, bool>
{
    private readonly IStandingRepository _standingRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteStandingHandler"/> class.
    /// </summary>
    /// <param name="standingRepository">The repository used to load and delete standings.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteStandingHandler(IStandingRepository standingRepository, IUnitOfWork unitOfWork)
    {
        _standingRepository = standingRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the standing identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no standing with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteStandingRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _standingRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Standing {request.Id} not found.");

            await _standingRepository.DeleteAsync(request.Id);

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
