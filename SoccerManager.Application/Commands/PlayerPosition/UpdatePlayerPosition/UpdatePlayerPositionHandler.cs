using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.PlayerPosition.UpdatePlayerPosition;

/// <summary>
/// Handles <see cref="UpdatePlayerPositionRequest"/> commands.
/// </summary>
public class UpdatePlayerPositionHandler : IRequestHandler<UpdatePlayerPositionRequest, bool>
{
    private readonly IPlayerPositionRepository _playerPositionRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePlayerPositionHandler"/> class.
    /// </summary>
    /// <param name="playerPositionRepository">The repository used to load and persist player positions.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdatePlayerPositionHandler(IPlayerPositionRepository playerPositionRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _playerPositionRepository = playerPositionRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing player position rating with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no player position with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdatePlayerPositionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var playerPosition = await _playerPositionRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"PlayerPosition {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdatePlayerPositionMapper.ApplyTo(request, playerPosition, now, _currentUser.UserId);

            await _playerPositionRepository.UpdateAsync(playerPosition);

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
