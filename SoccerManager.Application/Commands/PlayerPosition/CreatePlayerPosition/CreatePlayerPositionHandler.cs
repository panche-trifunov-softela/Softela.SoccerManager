using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.PlayerPosition.CreatePlayerPosition;

/// <summary>
/// Handles <see cref="CreatePlayerPositionRequest"/> commands.
/// </summary>
public class CreatePlayerPositionHandler : IRequestHandler<CreatePlayerPositionRequest, int>
{
    private readonly IPlayerPositionRepository _playerPositionRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePlayerPositionHandler"/> class.
    /// </summary>
    /// <param name="playerPositionRepository">The repository used to persist player positions.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreatePlayerPositionHandler(IPlayerPositionRepository playerPositionRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _playerPositionRepository = playerPositionRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new player position rating from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created player position.</returns>
    public async Task<int> Handle(CreatePlayerPositionRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var playerPosition = CreatePlayerPositionMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _playerPositionRepository.CreateAsync(playerPosition);

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
