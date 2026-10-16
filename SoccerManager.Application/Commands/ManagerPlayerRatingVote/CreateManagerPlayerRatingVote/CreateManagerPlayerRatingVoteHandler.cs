using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.ManagerPlayerRatingVote.CreateManagerPlayerRatingVote;

/// <summary>
/// Handles <see cref="CreateManagerPlayerRatingVoteRequest"/> commands.
/// </summary>
public class CreateManagerPlayerRatingVoteHandler : IRequestHandler<CreateManagerPlayerRatingVoteRequest, int>
{
    private readonly IManagerPlayerRatingVoteRepository _managerPlayerRatingVoteRepository;
    private readonly IManagerRepository _managerRepository;
    private readonly IPlayerRatingRepository _playerRatingRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateManagerPlayerRatingVoteHandler"/> class.
    /// </summary>
    /// <param name="managerPlayerRatingVoteRepository">The repository used to persist votes.</param>
    /// <param name="managerRepository">The repository used to find the caller's manager profile.</param>
    /// <param name="playerRatingRepository">The repository used to find the player's current rating.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateManagerPlayerRatingVoteHandler(IManagerPlayerRatingVoteRepository managerPlayerRatingVoteRepository, IManagerRepository managerRepository, IPlayerRatingRepository playerRatingRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _managerPlayerRatingVoteRepository = managerPlayerRatingVoteRepository;
        _managerRepository = managerRepository;
        _playerRatingRepository = playerRatingRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Casts a vote for the authenticated caller on the player's current rating.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created vote.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the caller has no manager profile or the player does not exist.</exception>
    public async Task<int> Handle(CreateManagerPlayerRatingVoteRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        // The validator already rejects both, so each is reachable only if the row disappears between
        // validation and here: the manager profile, or the player's current rating.
        var manager = await _managerRepository.GetByUserIdAsync(_currentUser.UserId)
            ?? throw new KeyNotFoundException("The caller has no manager profile.");

        var currentRating = await _playerRatingRepository.GetCurrentByPlayerIdAsync(request.PlayerId)
            ?? throw new KeyNotFoundException($"Player {request.PlayerId} not found.");

        // The vote belongs to the rating in force now, so a later rating, even one with the same value, starts a new round.
        var managerPlayerRatingVote = CreateManagerPlayerRatingVoteMapper.ToDomainEntity(request, manager.Id, currentRating.Id, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _managerPlayerRatingVoteRepository.CreateAsync(managerPlayerRatingVote);

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
