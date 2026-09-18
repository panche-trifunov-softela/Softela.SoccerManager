using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.MatchTeamStatistic.DeleteMatchTeamStatistic;

/// <summary>
/// Handles <see cref="DeleteMatchTeamStatisticRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteMatchTeamStatisticHandler : IRequestHandler<DeleteMatchTeamStatisticRequest, bool>
{
    private readonly IMatchTeamStatisticRepository _matchTeamStatisticRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteMatchTeamStatisticHandler"/> class.
    /// </summary>
    /// <param name="matchTeamStatisticRepository">The repository used to load and delete match team statistics.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteMatchTeamStatisticHandler(IMatchTeamStatisticRepository matchTeamStatisticRepository, IUnitOfWork unitOfWork)
    {
        _matchTeamStatisticRepository = matchTeamStatisticRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the match team statistic identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match team statistic with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteMatchTeamStatisticRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _matchTeamStatisticRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Match team statistic {request.Id} not found.");

            await _matchTeamStatisticRepository.DeleteAsync(request.Id);

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
