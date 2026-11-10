using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.PenaltyAfterExtraTime.DeletePenaltyAfterExtraTime;

/// <summary>
/// Handles <see cref="DeletePenaltyAfterExtraTimeRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeletePenaltyAfterExtraTimeHandler : IRequestHandler<DeletePenaltyAfterExtraTimeRequest, bool>
{
    private readonly IPenaltyAfterExtraTimeRepository _penaltyAfterExtraTimeRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePenaltyAfterExtraTimeHandler"/> class.
    /// </summary>
    /// <param name="penaltyAfterExtraTimeRepository">The repository used to load and delete penalties after extra time.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeletePenaltyAfterExtraTimeHandler(IPenaltyAfterExtraTimeRepository penaltyAfterExtraTimeRepository, IUnitOfWork unitOfWork)
    {
        _penaltyAfterExtraTimeRepository = penaltyAfterExtraTimeRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the penalty after extra time identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no penalty after extra time with the given identifier exists.</exception>
    public async Task<bool> Handle(DeletePenaltyAfterExtraTimeRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _penaltyAfterExtraTimeRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Penalty after extra time {request.Id} not found.");

            await _penaltyAfterExtraTimeRepository.DeleteAsync(request.Id);

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
