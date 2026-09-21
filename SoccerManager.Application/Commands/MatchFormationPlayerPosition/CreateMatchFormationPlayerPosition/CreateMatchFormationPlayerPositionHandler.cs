using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.MatchFormationPlayerPosition.CreateMatchFormationPlayerPosition;

/// <summary>
/// Handles <see cref="CreateMatchFormationPlayerPositionRequest"/> commands.
/// </summary>
public class CreateMatchFormationPlayerPositionHandler : IRequestHandler<CreateMatchFormationPlayerPositionRequest, int>
{
    private readonly IMatchFormationPlayerPositionRepository _matchFormationPlayerPositionRepository;
    private readonly IMatchFormationPlayerPositionSnapshotResolver _snapshotResolver;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMatchFormationPlayerPositionHandler"/> class.
    /// </summary>
    /// <param name="matchFormationPlayerPositionRepository">The repository used to persist match formation player positions.</param>
    /// <param name="snapshotResolver">The resolver that supplies the values snapshotted from the player's registration and position rating.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateMatchFormationPlayerPositionHandler(IMatchFormationPlayerPositionRepository matchFormationPlayerPositionRepository, IMatchFormationPlayerPositionSnapshotResolver snapshotResolver, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchFormationPlayerPositionRepository = matchFormationPlayerPositionRepository;
        _snapshotResolver = snapshotResolver;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Creates a new match formation player position from the given request, snapshotting the player's
    /// condition, quality and suspension status.
    /// </summary>
    /// <param name="request">The create request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The identifier of the created match formation player position.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the match, its season, the player position or the player's league registration does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the player is registered with a different team in the match's league.</exception>
    public async Task<int> Handle(CreateMatchFormationPlayerPositionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Resolved inside the transaction so the snapshot and the insert see the same data.
            var snapshot = await _snapshotResolver.ResolveAsync(request.MatchId, request.TeamId, request.PlayerPositionId);

            var now = DateTime.UtcNow;

            var matchFormationPlayerPosition = CreateMatchFormationPlayerPositionMapper.ToDomainEntity(request, snapshot, now, _currentUser.UserId);

            var id = await _matchFormationPlayerPositionRepository.CreateAsync(matchFormationPlayerPosition);

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
