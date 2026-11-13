using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransfer.CreateLeagueTeamPlayerTransfer;

/// <summary>
/// Handles <see cref="CreateLeagueTeamPlayerTransferRequest"/> commands.
/// </summary>
public class CreateLeagueTeamPlayerTransferHandler : IRequestHandler<CreateLeagueTeamPlayerTransferRequest, int>
{
    private readonly ILeagueTeamPlayerTransferRepository _leagueTeamPlayerTransferRepository;
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;
    private readonly IPlayerDealService _playerDealService;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamPlayerTransferHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferRepository">The repository used to persist transfers.</param>
    /// <param name="leagueTeamPlayerRepository">The repository used to load the transferred squad row.</param>
    /// <param name="playerDealService">The service used to check that the clubs and the season belong to the player's league.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateLeagueTeamPlayerTransferHandler(ILeagueTeamPlayerTransferRepository leagueTeamPlayerTransferRepository, ILeagueTeamPlayerRepository leagueTeamPlayerRepository, IPlayerDealService playerDealService, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerTransferRepository = leagueTeamPlayerTransferRepository;
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
        _playerDealService = playerDealService;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new transfer from the given request. The selling club is the club of the transferred squad row.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created transfer.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the squad row, the buying club or the season does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the buying club is the selling club or is not in its league, or the season is not in its league.</exception>
    public async Task<int> Handle(CreateLeagueTeamPlayerTransferRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var leagueTeamPlayer = await _leagueTeamPlayerRepository.GetByIdAsync(request.LeagueTeamPlayerId)
                ?? throw new KeyNotFoundException($"League team player {request.LeagueTeamPlayerId} not found.");

            var fromLeagueTeamId = leagueTeamPlayer.LeagueTeamId;

            await _playerDealService.EnsureDealIsValidAsync(fromLeagueTeamId, request.ToLeagueTeamId, request.SeasonId);

            var now = DateTime.UtcNow;

            var leagueTeamPlayerTransfer = CreateLeagueTeamPlayerTransferMapper.ToDomainEntity(request, fromLeagueTeamId, now, _currentUser.UserId);

            var id = await _leagueTeamPlayerTransferRepository.CreateAsync(leagueTeamPlayerTransfer);

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
