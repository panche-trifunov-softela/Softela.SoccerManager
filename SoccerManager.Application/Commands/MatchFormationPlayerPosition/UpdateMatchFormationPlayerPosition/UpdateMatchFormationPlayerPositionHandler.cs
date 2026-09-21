using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Commands.MatchFormationPlayerPosition.UpdateMatchFormationPlayerPosition;

/// <summary>
/// Handles <see cref="UpdateMatchFormationPlayerPositionRequest"/> commands.
/// </summary>
public class UpdateMatchFormationPlayerPositionHandler : IRequestHandler<UpdateMatchFormationPlayerPositionRequest, bool>
{
    private readonly IMatchFormationPlayerPositionRepository _matchFormationPlayerPositionRepository;
    private readonly IMatchFormationPlayerPositionSnapshotResolver _snapshotResolver;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMatchFormationPlayerPositionHandler"/> class.
    /// </summary>
    /// <param name="matchFormationPlayerPositionRepository">The repository used to load and persist match formation player positions.</param>
    /// <param name="snapshotResolver">The resolver that supplies the values snapshotted from the player's registration and position rating.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateMatchFormationPlayerPositionHandler(IMatchFormationPlayerPositionRepository matchFormationPlayerPositionRepository, IMatchFormationPlayerPositionSnapshotResolver snapshotResolver, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchFormationPlayerPositionRepository = matchFormationPlayerPositionRepository;
        _snapshotResolver = snapshotResolver;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing match formation player position with the values from the given request,
    /// re-snapshotting the player's condition, quality and suspension status.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match formation player position with the given identifier exists, or when the match, its season, the player position or the player's league registration does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the player is registered with a different team in the match's league.</exception>
    public async Task<bool> Handle(UpdateMatchFormationPlayerPositionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var matchFormationPlayerPosition = await _matchFormationPlayerPositionRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Match formation player position {request.Id} not found.");

            // Re-snapshotted on every update: the request may move a different player into the slot, and the copied values follow it.
            var snapshot = await _snapshotResolver.ResolveAsync(request.MatchId, request.TeamId, request.PlayerPositionId);

            var now = DateTime.UtcNow;

            UpdateMatchFormationPlayerPositionMapper.ApplyTo(request, snapshot, matchFormationPlayerPosition, now, _currentUser.UserId);

            await _matchFormationPlayerPositionRepository.UpdateAsync(matchFormationPlayerPosition);

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
