using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.LeagueTeamPlayer.UpdateLeagueTeamPlayer;

/// <summary>
/// Handles <see cref="UpdateLeagueTeamPlayerRequest"/> commands.
/// </summary>
public class UpdateLeagueTeamPlayerHandler : IRequestHandler<UpdateLeagueTeamPlayerRequest, bool>
{
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueTeamPlayerHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerRepository">The repository used to load and persist league team players.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateLeagueTeamPlayerHandler(ILeagueTeamPlayerRepository leagueTeamPlayerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing league team player with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team player with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdateLeagueTeamPlayerRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var leagueTeamPlayer = await _leagueTeamPlayerRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"League team player {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdateLeagueTeamPlayerMapper.ApplyTo(request, leagueTeamPlayer, now, _currentUser.UserId);

            await _leagueTeamPlayerRepository.UpdateAsync(leagueTeamPlayer);

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
