using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.MatchTeamTactic.DeleteMatchTeamTactic;

/// <summary>
/// Handles <see cref="DeleteMatchTeamTacticRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteMatchTeamTacticHandler : IRequestHandler<DeleteMatchTeamTacticRequest, bool>
{
    private readonly IMatchTeamTacticRepository _matchTeamTacticRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteMatchTeamTacticHandler"/> class.
    /// </summary>
    /// <param name="matchTeamTacticRepository">The repository used to load and delete match team tactics.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteMatchTeamTacticHandler(IMatchTeamTacticRepository matchTeamTacticRepository, IUnitOfWork unitOfWork)
    {
        _matchTeamTacticRepository = matchTeamTacticRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the match team tactic identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match team tactic with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteMatchTeamTacticRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _matchTeamTacticRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Match team tactic {request.Id} not found.");

            await _matchTeamTacticRepository.DeleteAsync(request.Id);

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
