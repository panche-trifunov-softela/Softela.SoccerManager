using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.PlayerRating.DeletePlayerRating;

/// <summary>
/// Handles <see cref="DeletePlayerRatingRequest"/> commands. This performs a hard delete, as the
/// entity carries no soft-delete flag; the votes cast on the rating are deleted with it.
/// </summary>
public class DeletePlayerRatingHandler : IRequestHandler<DeletePlayerRatingRequest, bool>
{
    private readonly IPlayerRatingRepository _playerRatingRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeletePlayerRatingHandler"/> class.
    /// </summary>
    /// <param name="playerRatingRepository">The repository used to load and delete player ratings.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeletePlayerRatingHandler(IPlayerRatingRepository playerRatingRepository, IUnitOfWork unitOfWork)
    {
        _playerRatingRepository = playerRatingRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the past player rating identified by the given request, together with the votes cast on it.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no player rating with the given identifier exists.</exception>
    /// <exception cref="ValidationException">Thrown when the rating is the player's current one.</exception>
    public async Task<bool> Handle(DeletePlayerRatingRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var playerRating = await _playerRatingRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Player rating {request.Id} not found.");

            // Every player keeps exactly one current rating, so only a history row can go.
            if (playerRating.IsCurrent)
            {
                throw new ValidationException(new[]
                {
                    new ValidationFailure(nameof(DeletePlayerRatingRequest.Id), "The current rating cannot be deleted; post a new rating instead."),
                });
            }

            await _playerRatingRepository.DeleteAsync(request.Id);

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
