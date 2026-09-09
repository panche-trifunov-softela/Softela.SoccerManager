using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.Player.CreatePlayer;

/// <summary>
/// Handles <see cref="CreatePlayerRequest"/> commands.
/// </summary>
public class CreatePlayerHandler : IRequestHandler<CreatePlayerRequest, int>
{
    private readonly IPlayerRepository _playerRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreatePlayerHandler"/> class.
    /// </summary>
    /// <param name="playerRepository">The repository used to persist players.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreatePlayerHandler(IPlayerRepository playerRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _playerRepository = playerRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new player from the given request.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created player.</returns>
    public async Task<int> Handle(CreatePlayerRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var player = CreatePlayerMapper.ToDomainEntity(request, now, _currentUser.UserId);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var id = await _playerRepository.CreateAsync(player);

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
