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
    private readonly IPlayerRepository _playerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateManagerPlayerRatingVoteHandler"/> class.
    /// </summary>
    /// <param name="managerPlayerRatingVoteRepository">The repository used to persist votes.</param>
    /// <param name="managerRepository">The repository used to find the caller's manager profile.</param>
    /// <param name="playerRepository">The repository used to read the player's current rating.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateManagerPlayerRatingVoteHandler(IManagerPlayerRatingVoteRepository managerPlayerRatingVoteRepository, IManagerRepository managerRepository, IPlayerRepository playerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _managerPlayerRatingVoteRepository = managerPlayerRatingVoteRepository;
        _managerRepository = managerRepository;
        _playerRepository = playerRepository;
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
        // validation and here.
        var manager = await _managerRepository.GetByUserIdAsync(_currentUser.UserId)
            ?? throw new KeyNotFoundException("The caller has no manager profile.");

        var player = await _playerRepository.GetByIdAsync(request.PlayerId)
            ?? throw new KeyNotFoundException($"Player {request.PlayerId} not found.");

        // The rating is copied now, so the vote keeps the rating it was cast against after the player's rating changes.
        var managerPlayerRatingVote = CreateManagerPlayerRatingVoteMapper.ToDomainEntity(request, manager.Id, player.Rating, now, _currentUser.UserId);

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
