using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Player.UpdatePlayer;

/// <summary>
/// Handles <see cref="UpdatePlayerRequest"/> commands.
/// </summary>
public class UpdatePlayerHandler : IRequestHandler<UpdatePlayerRequest, bool>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdatePlayerHandler"/> class.
    /// </summary>
    /// <param name="playerRepository">The repository used to load and persist players.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdatePlayerHandler(IPlayerRepository playerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing player with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no player with the given identifier exists.</exception>
    public async Task<bool> Handle(UpdatePlayerRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var player = await _playerRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Player {request.Id} not found.");

            var now = DateTime.UtcNow;

            UpdatePlayerMapper.ApplyTo(request, player, now, _currentUser.UserId);

            await _playerRepository.UpdateAsync(player);

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
