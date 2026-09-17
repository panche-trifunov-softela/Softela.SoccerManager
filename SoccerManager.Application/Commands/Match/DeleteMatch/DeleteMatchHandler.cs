using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Match.DeleteMatch;

/// <summary>
/// Handles <see cref="DeleteMatchRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteMatchHandler : IRequestHandler<DeleteMatchRequest, bool>
{
    private readonly IMatchRepository _matchRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteMatchHandler"/> class.
    /// </summary>
    /// <param name="matchRepository">The repository used to load and delete matches.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteMatchHandler(IMatchRepository matchRepository, IUnitOfWork unitOfWork)
    {
        _matchRepository = matchRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the match identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteMatchRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _matchRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Match {request.Id} not found.");

            await _matchRepository.DeleteAsync(request.Id);

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
