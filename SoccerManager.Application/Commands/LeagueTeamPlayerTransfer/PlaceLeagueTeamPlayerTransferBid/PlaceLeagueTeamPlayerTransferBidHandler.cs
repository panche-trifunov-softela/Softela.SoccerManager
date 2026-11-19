using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.PlaceLeagueTeamPlayerTransferBid;

/// <summary>
/// Handles <see cref="PlaceLeagueTeamPlayerTransferBidRequest"/> commands.
/// </summary>
public class PlaceLeagueTeamPlayerTransferBidHandler : IRequestHandler<PlaceLeagueTeamPlayerTransferBidRequest, int>
{
    private readonly ILeagueTeamPlayerTransferRepository _leagueTeamPlayerTransferRepository;
    private readonly ILeagueTeamPlayerTransferSwapRepository _leagueTeamPlayerTransferSwapRepository;
    private readonly IPlayerDealService _playerDealService;
    private readonly IClubAccess _clubAccess;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaceLeagueTeamPlayerTransferBidHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferRepository">The repository used to persist the bid as a pending transfer.</param>
    /// <param name="leagueTeamPlayerTransferSwapRepository">The repository used to persist the players offered in the swap.</param>
    /// <param name="playerDealService">The service used to resolve the bid's target and check the bid against the rules of the league.</param>
    /// <param name="clubAccess">The check that the caller may bid for the club.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public PlaceLeagueTeamPlayerTransferBidHandler(ILeagueTeamPlayerTransferRepository leagueTeamPlayerTransferRepository, ILeagueTeamPlayerTransferSwapRepository leagueTeamPlayerTransferSwapRepository, IPlayerDealService playerDealService, IClubAccess clubAccess, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerTransferRepository = leagueTeamPlayerTransferRepository;
        _leagueTeamPlayerTransferSwapRepository = leagueTeamPlayerTransferSwapRepository;
        _playerDealService = playerDealService;
        _clubAccess = clubAccess;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Places a pending transfer bid by the given club for the given player, with the players it offers in a swap. The
    /// selling club is the club the player is registered at, and the season is the league's latest one.
    /// </summary>
    /// <param name="request">The bid request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created transfer.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the bidding club does not exist.</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the bidding club's current manager.</exception>
    /// <exception cref="ValidationException">Thrown when the player is in the bidding club's squad or at no club of the league, the league has no season, the club already has a pending bid for the player, the player already has an agreed deal, an offered player is not in the bidding club's squad, or an offered player already has an agreed deal.</exception>
    public async Task<int> Handle(PlaceLeagueTeamPlayerTransferBidRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _clubAccess.EnsureCanManageClubAsync(request.LeagueTeamId);

            var target = await _playerDealService.GetBidTargetAsync(request.LeagueTeamId, request.PlayerId);

            // The check runs before any insert, so a refused swap leaves nothing behind.
            if (request.SwapLeagueTeamPlayerIds.Any(id => !target.SquadLeagueTeamPlayerIds.Contains(id)))
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(nameof(request.SwapLeagueTeamPlayerIds), "A swapped player is not in the club's squad."),
                });
            }

            await _playerDealService.EnsureSwapPlayersHaveNoAgreedDealAsync(request.SwapLeagueTeamPlayerIds);

            var now = DateTime.UtcNow;

            var leagueTeamPlayerTransfer = PlaceLeagueTeamPlayerTransferBidMapper.ToDomainEntity(request, target, now, _currentUser.UserId);

            var id = await _leagueTeamPlayerTransferRepository.CreateAsync(leagueTeamPlayerTransfer);

            foreach (var swapLeagueTeamPlayerId in request.SwapLeagueTeamPlayerIds)
            {
                var swap = PlaceLeagueTeamPlayerTransferBidMapper.ToSwap(id, swapLeagueTeamPlayerId, now, _currentUser.UserId);

                await _leagueTeamPlayerTransferSwapRepository.CreateAsync(swap);
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return id;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
