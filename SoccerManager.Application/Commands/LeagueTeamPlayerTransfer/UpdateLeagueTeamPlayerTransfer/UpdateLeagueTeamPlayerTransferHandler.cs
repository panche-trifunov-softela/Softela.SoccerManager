using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.UpdateLeagueTeamPlayerTransfer;

/// <summary>
/// Handles <see cref="UpdateLeagueTeamPlayerTransferRequest"/> commands.
/// </summary>
public class UpdateLeagueTeamPlayerTransferHandler : IRequestHandler<UpdateLeagueTeamPlayerTransferRequest, bool>
{
    private readonly ILeagueTeamPlayerTransferRepository _leagueTeamPlayerTransferRepository;
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;
    private readonly IPlayerDealService _playerDealService;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueTeamPlayerTransferHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferRepository">The repository used to load and persist transfers.</param>
    /// <param name="leagueTeamPlayerRepository">The repository used to load the squad row a transfer moves to.</param>
    /// <param name="playerDealService">The service used to check that the clubs and the season belong to the player's league.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateLeagueTeamPlayerTransferHandler(ILeagueTeamPlayerTransferRepository leagueTeamPlayerTransferRepository, ILeagueTeamPlayerRepository leagueTeamPlayerRepository, IPlayerDealService playerDealService, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerTransferRepository = leagueTeamPlayerTransferRepository;
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        _playerDealService = playerDealService;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing transfer with the values from the given request. The selling club stays the one stored with the
    /// transfer, unless the request names another squad row: then it is the club of that squad row.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no transfer with the given identifier exists, or the squad row, the buying club or the season does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the buying club is the selling club or is not in its league, or the season is not in its league.</exception>
    public async Task<bool> Handle(UpdateLeagueTeamPlayerTransferRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var leagueTeamPlayerTransfer = await _leagueTeamPlayerTransferRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"League team player transfer {request.Id} not found.");

            var fromLeagueTeamId = leagueTeamPlayerTransfer.FromLeagueTeamId;

            // Only a transfer that moves to another squad row takes its selling club from that row; otherwise the stored
            // club stays, even when the player has since moved on.
            if (request.LeagueTeamPlayerId != leagueTeamPlayerTransfer.LeagueTeamPlayerId)
            {
                var leagueTeamPlayer = await _leagueTeamPlayerRepository.GetByIdAsync(request.LeagueTeamPlayerId)
                    ?? throw new KeyNotFoundException($"League team player {request.LeagueTeamPlayerId} not found.");

                fromLeagueTeamId = leagueTeamPlayer.LeagueTeamId;
            }

            await _playerDealService.EnsureDealIsValidAsync(fromLeagueTeamId, request.ToLeagueTeamId, request.SeasonId);

            var now = DateTime.UtcNow;

            UpdateLeagueTeamPlayerTransferMapper.ApplyTo(request, leagueTeamPlayerTransfer, fromLeagueTeamId, now, _currentUser.UserId);

            await _leagueTeamPlayerTransferRepository.UpdateAsync(leagueTeamPlayerTransfer);

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
