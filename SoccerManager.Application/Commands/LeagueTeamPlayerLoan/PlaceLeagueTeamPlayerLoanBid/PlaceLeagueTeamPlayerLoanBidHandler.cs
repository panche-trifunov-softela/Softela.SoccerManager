using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerLoan.PlaceLeagueTeamPlayerLoanBid;

/// <summary>
/// Handles <see cref="PlaceLeagueTeamPlayerLoanBidRequest"/> commands.
/// </summary>
public class PlaceLeagueTeamPlayerLoanBidHandler : IRequestHandler<PlaceLeagueTeamPlayerLoanBidRequest, int>
{
    private readonly ILeagueTeamPlayerLoanRepository _leagueTeamPlayerLoanRepository;
    private readonly IPlayerDealService _playerDealService;
    private readonly IClubAccess _clubAccess;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlaceLeagueTeamPlayerLoanBidHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerLoanRepository">The repository used to persist the bid as a pending loan.</param>
    /// <param name="playerDealService">The service used to resolve the bid's target and check the bid against the rules of the league.</param>
    /// <param name="clubAccess">The check that the caller may bid for the club.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public PlaceLeagueTeamPlayerLoanBidHandler(ILeagueTeamPlayerLoanRepository leagueTeamPlayerLoanRepository, IPlayerDealService playerDealService, IClubAccess clubAccess, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerLoanRepository = leagueTeamPlayerLoanRepository;
        _playerDealService = playerDealService;
        _clubAccess = clubAccess;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Places a pending loan bid by the given club for the given player. The lending club is the club the player is
    /// registered at, and the season is the league's latest one.
    /// </summary>
    /// <param name="request">The bid request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created loan.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the bidding club does not exist.</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the bidding club's current manager.</exception>
    /// <exception cref="ValidationException">Thrown when the player is in the bidding club's squad or at no club of the league, the league has no season, the club already has a pending bid for the player, or the player already has an agreed deal.</exception>
    public async Task<int> Handle(PlaceLeagueTeamPlayerLoanBidRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _clubAccess.EnsureCanManageClubAsync(request.LeagueTeamId);

            var target = await _playerDealService.GetBidTargetAsync(request.LeagueTeamId, request.PlayerId);

            var now = DateTime.UtcNow;

            var leagueTeamPlayerLoan = PlaceLeagueTeamPlayerLoanBidMapper.ToDomainEntity(request, target, now, _currentUser.UserId);

            var id = await _leagueTeamPlayerLoanRepository.CreateAsync(leagueTeamPlayerLoan);

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
