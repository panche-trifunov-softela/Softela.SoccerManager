using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerLoan.UpdateLeagueTeamPlayerLoan;

/// <summary>
/// Handles <see cref="UpdateLeagueTeamPlayerLoanRequest"/> commands.
/// </summary>
public class UpdateLeagueTeamPlayerLoanHandler : IRequestHandler<UpdateLeagueTeamPlayerLoanRequest, bool>
{
    private readonly ILeagueTeamPlayerLoanRepository _leagueTeamPlayerLoanRepository;
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;
    private readonly IPlayerDealService _playerDealService;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateLeagueTeamPlayerLoanHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerLoanRepository">The repository used to load and persist loans.</param>
    /// <param name="leagueTeamPlayerRepository">The repository used to load the loaned player's squad row.</param>
    /// <param name="playerDealService">The service used to check the loan club and the season against the player's league.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateLeagueTeamPlayerLoanHandler(ILeagueTeamPlayerLoanRepository leagueTeamPlayerLoanRepository, ILeagueTeamPlayerRepository leagueTeamPlayerRepository, IPlayerDealService playerDealService, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerLoanRepository = leagueTeamPlayerLoanRepository;
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        _playerDealService = playerDealService;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing loan with the values from the given request. The loan club and the season are checked again
    /// only when the squad row, the loan club or the season changes, so changing only the fee, the terms or the status
    /// never fails on the strength of the clubs or the season.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no loan with the given identifier exists, or the squad row, the loan club or the season does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the loan club is the lending club, or the loan club or the season is not in the player's league.</exception>
    public async Task<bool> Handle(UpdateLeagueTeamPlayerLoanRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var leagueTeamPlayerLoan = await _leagueTeamPlayerLoanRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"League team player loan {request.Id} not found.");

            var dealChanged = request.LeagueTeamPlayerId != leagueTeamPlayerLoan.LeagueTeamPlayerId
                || request.ToLeagueTeamId != leagueTeamPlayerLoan.ToLeagueTeamId
                || request.SeasonId != leagueTeamPlayerLoan.SeasonId;

            if (dealChanged)
            {
                var leagueTeamPlayer = await _leagueTeamPlayerRepository.GetByIdAsync(request.LeagueTeamPlayerId)
                    ?? throw new KeyNotFoundException($"League team player {request.LeagueTeamPlayerId} not found.");

                await _playerDealService.EnsureDealIsValidAsync(leagueTeamPlayer.LeagueTeamId, request.ToLeagueTeamId, request.SeasonId);
            }

            var now = DateTime.UtcNow;

            UpdateLeagueTeamPlayerLoanMapper.ApplyTo(request, leagueTeamPlayerLoan, now, _currentUser.UserId);

            await _leagueTeamPlayerLoanRepository.UpdateAsync(leagueTeamPlayerLoan);

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
