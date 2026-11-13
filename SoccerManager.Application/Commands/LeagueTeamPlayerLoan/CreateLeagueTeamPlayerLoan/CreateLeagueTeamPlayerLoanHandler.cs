using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerLoan.CreateLeagueTeamPlayerLoan;

/// <summary>
/// Handles <see cref="CreateLeagueTeamPlayerLoanRequest"/> commands.
/// </summary>
public class CreateLeagueTeamPlayerLoanHandler : IRequestHandler<CreateLeagueTeamPlayerLoanRequest, int>
{
    private readonly ILeagueTeamPlayerLoanRepository _leagueTeamPlayerLoanRepository;
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;
    private readonly IPlayerDealService _playerDealService;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamPlayerLoanHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerLoanRepository">The repository used to persist loans.</param>
    /// <param name="leagueTeamPlayerRepository">The repository used to load the loaned player's squad row.</param>
    /// <param name="playerDealService">The service used to check the loan club and the season against the player's league.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateLeagueTeamPlayerLoanHandler(ILeagueTeamPlayerLoanRepository leagueTeamPlayerLoanRepository, ILeagueTeamPlayerRepository leagueTeamPlayerRepository, IPlayerDealService playerDealService, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerLoanRepository = leagueTeamPlayerLoanRepository;
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        _playerDealService = playerDealService;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new loan from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created loan.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the squad row, the loan club or the season does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the loan club is the lending club, or the loan club or the season is not in the player's league.</exception>
    public async Task<int> Handle(CreateLeagueTeamPlayerLoanRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var leagueTeamPlayerLoan = CreateLeagueTeamPlayerLoanMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var leagueTeamPlayer = await _leagueTeamPlayerRepository.GetByIdAsync(request.LeagueTeamPlayerId)
                ?? throw new KeyNotFoundException($"League team player {request.LeagueTeamPlayerId} not found.");

            await _playerDealService.EnsureDealIsValidAsync(leagueTeamPlayer.LeagueTeamId, request.ToLeagueTeamId, request.SeasonId);

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
