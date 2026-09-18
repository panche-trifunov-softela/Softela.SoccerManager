using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamPlayer.DeleteLeagueTeamPlayer;

/// <summary>
/// Handles <see cref="DeleteLeagueTeamPlayerRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag.
/// </summary>
public class DeleteLeagueTeamPlayerHandler : IRequestHandler<DeleteLeagueTeamPlayerRequest, bool>
{
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLeagueTeamPlayerHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerRepository">The repository used to load and delete league team players.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteLeagueTeamPlayerHandler(ILeagueTeamPlayerRepository leagueTeamPlayerRepository, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the league team player identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team player with the given identifier exists.</exception>
    public async Task<bool> Handle(DeleteLeagueTeamPlayerRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await _leagueTeamPlayerRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"League team player {request.Id} not found.");

            await _leagueTeamPlayerRepository.DeleteAsync(request.Id);

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
