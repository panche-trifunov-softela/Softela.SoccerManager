using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.DeleteLeagueTeamPlayerTransfer;

/// <summary>
/// Handles <see cref="DeleteLeagueTeamPlayerTransferRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag; the transfer's swap rows go with it.
/// </summary>
public class DeleteLeagueTeamPlayerTransferHandler : IRequestHandler<DeleteLeagueTeamPlayerTransferRequest, bool>
{
    private readonly ILeagueTeamPlayerTransferRepository _leagueTeamPlayerTransferRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLeagueTeamPlayerTransferHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferRepository">The repository used to load and delete transfers.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteLeagueTeamPlayerTransferHandler(ILeagueTeamPlayerTransferRepository leagueTeamPlayerTransferRepository, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerTransferRepository = leagueTeamPlayerTransferRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the transfer identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no transfer with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteLeagueTeamPlayerTransferRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _leagueTeamPlayerTransferRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"League team player transfer {request.Id} not found.");

            await _leagueTeamPlayerTransferRepository.DeleteAsync(request.Id);

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
