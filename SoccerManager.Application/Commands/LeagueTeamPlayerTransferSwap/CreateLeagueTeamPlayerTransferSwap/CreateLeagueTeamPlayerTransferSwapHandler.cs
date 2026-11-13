using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerTransferSwap.CreateLeagueTeamPlayerTransferSwap;

/// <summary>
/// Handles <see cref="CreateLeagueTeamPlayerTransferSwapRequest"/> commands.
/// </summary>
public class CreateLeagueTeamPlayerTransferSwapHandler : IRequestHandler<CreateLeagueTeamPlayerTransferSwapRequest, int>
{
    private readonly ILeagueTeamPlayerTransferSwapRepository _leagueTeamPlayerTransferSwapRepository;
    private readonly IPlayerDealService _playerDealService;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateLeagueTeamPlayerTransferSwapHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerTransferSwapRepository">The repository used to persist swaps.</param>
    /// <param name="playerDealService">The service that checks the swapped player against the transfer.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateLeagueTeamPlayerTransferSwapHandler(ILeagueTeamPlayerTransferSwapRepository leagueTeamPlayerTransferSwapRepository, IPlayerDealService playerDealService, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _leagueTeamPlayerTransferSwapRepository = leagueTeamPlayerTransferSwapRepository;
        _playerDealService = playerDealService;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new swap from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created swap.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the transfer or the swapped player's squad row does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the swapped player is the sold player, or is not in the buying club's squad.</exception>
    public async Task<int> Handle(CreateLeagueTeamPlayerTransferSwapRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var leagueTeamPlayerTransferSwap = CreateLeagueTeamPlayerTransferSwapMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _playerDealService.EnsureSwapIsValidAsync(request.LeagueTeamPlayerTransferId, request.LeagueTeamPlayerId);

            var id = await _leagueTeamPlayerTransferSwapRepository.CreateAsync(leagueTeamPlayerTransferSwap);

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
