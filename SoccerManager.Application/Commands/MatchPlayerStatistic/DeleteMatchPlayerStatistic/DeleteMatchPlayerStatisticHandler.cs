using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.MatchPlayerStatistic.DeleteMatchPlayerStatistic;

/// <summary>
/// Handles <see cref="DeleteMatchPlayerStatisticRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteMatchPlayerStatisticHandler : IRequestHandler<DeleteMatchPlayerStatisticRequest, bool>
{
    private readonly IMatchPlayerStatisticRepository _matchPlayerStatisticRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteMatchPlayerStatisticHandler"/> class.
    /// </summary>
    /// <param name="matchPlayerStatisticRepository">The repository used to load and delete match player statistics.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteMatchPlayerStatisticHandler(IMatchPlayerStatisticRepository matchPlayerStatisticRepository, IUnitOfWork unitOfWork)
    {
        _matchPlayerStatisticRepository = matchPlayerStatisticRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the match player statistic identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match player statistic with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteMatchPlayerStatisticRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _matchPlayerStatisticRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Match player statistic {request.Id} not found.");

            await _matchPlayerStatisticRepository.DeleteAsync(request.Id);

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
