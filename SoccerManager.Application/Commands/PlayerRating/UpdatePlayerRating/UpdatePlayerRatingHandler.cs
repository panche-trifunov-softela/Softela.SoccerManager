using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.PlayerRating.UpdatePlayerRating;

/// <summary>
/// Handles <see cref="UpdatePlayerRatingRequest"/> commands.
/// </summary>
public class UpdatePlayerRatingHandler : IRequestHandler<UpdatePlayerRatingRequest, bool>
{
    private readonly IPlayerRatingRepository _playerRatingRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePlayerRatingHandler"/> class.
    /// </summary>
    /// <param name="playerRatingRepository">The repository used to load and persist player ratings.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdatePlayerRatingHandler(IPlayerRatingRepository playerRatingRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _playerRatingRepository = playerRatingRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Corrects the value of a rating; whether it is current, and the votes cast on it, do not change.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no player rating with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdatePlayerRatingRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var playerRating = await _playerRatingRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Player rating {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdatePlayerRatingMapper.ApplyTo(request, playerRating, now, _currentUser.UserId);

            await _playerRatingRepository.UpdateAsync(playerRating);

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
