using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.LeagueTeamPlayer.DeleteLeagueTeamPlayer;

/// <summary>
/// Handles <see cref="DeleteLeagueTeamPlayerRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag. Releasing a player also removes them from the club's lineups for
/// matches that have not started.
/// </summary>
public class DeleteLeagueTeamPlayerHandler : IRequestHandler<DeleteLeagueTeamPlayerRequest, bool>
{
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;
    private readonly IMatchFormationPlayerPositionRepository _matchFormationPlayerPositionRepository;
    private readonly IClubAccess _clubAccess;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteLeagueTeamPlayerHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerRepository">The repository used to load and delete league team players.</param>
    /// <param name="matchFormationPlayerPositionRepository">The repository used to remove the released player from upcoming lineups.</param>
    /// <param name="clubAccess">The check that the caller may change the player's club.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteLeagueTeamPlayerHandler(ILeagueTeamPlayerRepository leagueTeamPlayerRepository, IMatchFormationPlayerPositionRepository matchFormationPlayerPositionRepository, IClubAccess clubAccess, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        _matchFormationPlayerPositionRepository = matchFormationPlayerPositionRepository;
        _clubAccess = clubAccess;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the league team player identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team player with the given identifier exists.</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    public async Task<bool> Handle(DeleteLeagueTeamPlayerRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var leagueTeamPlayer = await _leagueTeamPlayerRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"League team player {request.Id} not found.");

            await _clubAccess.EnsureCanManageClubAsync(leagueTeamPlayer.LeagueTeamId);

            // Done for every caller, operators included: a released player must not stay in a lineup for a match still to come.
            await _matchFormationPlayerPositionRepository.DeleteUnstartedByLeagueTeamAndPlayerAsync(leagueTeamPlayer.LeagueTeamId, leagueTeamPlayer.PlayerId);

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
