using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.PlayerRating.CreatePlayerRating;

/// <summary>
/// Handles <see cref="CreatePlayerRatingRequest"/> commands.
/// </summary>
public class CreatePlayerRatingHandler : IRequestHandler<CreatePlayerRatingRequest, int>
{
    private readonly IPlayerRatingRepository _playerRatingRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePlayerRatingHandler"/> class.
    /// </summary>
    /// <param name="playerRatingRepository">The repository used to persist player ratings.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreatePlayerRatingHandler(IPlayerRatingRepository playerRatingRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _playerRatingRepository = playerRatingRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Makes the new rating the player's current one; the previous current rating is kept as history.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created player rating.</returns>
    public async Task<int> Handle(CreatePlayerRatingRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var playerRating = CreatePlayerRatingMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _playerRatingRepository.CreateAsync(playerRating);

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
