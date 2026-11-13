using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransferSwap.DeleteLeagueTeamPlayerTransferSwap;

/// <summary>
/// Handles <see cref="DeleteLeagueTeamPlayerTransferSwapRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteLeagueTeamPlayerTransferSwapHandler : IRequestHandler<DeleteLeagueTeamPlayerTransferSwapRequest, bool>
{
    private readonly ILeagueTeamPlayerTransferSwapRepository _leagueTeamPlayerTransferSwapRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLeagueTeamPlayerTransferSwapHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferSwapRepository">The repository used to load and delete swaps.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteLeagueTeamPlayerTransferSwapHandler(ILeagueTeamPlayerTransferSwapRepository leagueTeamPlayerTransferSwapRepository, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerTransferSwapRepository = leagueTeamPlayerTransferSwapRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the swap identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no swap with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteLeagueTeamPlayerTransferSwapRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _leagueTeamPlayerTransferSwapRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"League team player transfer swap {request.Id} not found.");

            await _leagueTeamPlayerTransferSwapRepository.DeleteAsync(request.Id);

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
