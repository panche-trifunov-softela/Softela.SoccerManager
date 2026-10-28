using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.MatchFormationPlayerPosition.UpdateMatchFormationPlayerPosition;

/// <summary>
/// Handles <see cref="UpdateMatchFormationPlayerPositionRequest"/> commands.
/// </summary>
public class UpdateMatchFormationPlayerPositionHandler : IRequestHandler<UpdateMatchFormationPlayerPositionRequest, bool>
{
    private readonly IMatchFormationPlayerPositionRepository _matchFormationPlayerPositionRepository;
    private readonly IMatchFormationPlayerPositionSnapshotResolver _snapshotResolver;
    private readonly IMatchFormationPlayerPositionRules _rules;
    private readonly IClubAccess _clubAccess;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMatchFormationPlayerPositionHandler"/> class.
    /// </summary>
    /// <param name="matchFormationPlayerPositionRepository">The repository used to load and persist match formation player positions.</param>
    /// <param name="snapshotResolver">The resolver that supplies the values snapshotted from the player's registration and position rating.</param>
    /// <param name="rules">The rules a lineup slot must obey for every caller.</param>
    /// <param name="clubAccess">The check that the caller may change the club's lineup for the match.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateMatchFormationPlayerPositionHandler(IMatchFormationPlayerPositionRepository matchFormationPlayerPositionRepository, IMatchFormationPlayerPositionSnapshotResolver snapshotResolver, IMatchFormationPlayerPositionRules rules, IClubAccess clubAccess, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchFormationPlayerPositionRepository = matchFormationPlayerPositionRepository;
        _snapshotResolver = snapshotResolver;
        _rules = rules;
        _clubAccess = clubAccess;
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
    /// <exception cref="ValidationException">Thrown when the player is registered with a different team in the match's league, is suspended or already has a slot in the lineup, or when a non-operator caller's club does not play in the match or the match has already started.</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager, or when a non-operator caller moves the slot to another match or club.</exception>
    public async Task<bool> Handle(UpdateMatchFormationPlayerPositionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var matchFormationPlayerPosition = await _matchFormationPlayerPositionRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Match formation player position {request.Id} not found.");

            // The row's own match and club are checked, not the request's: the stored values decide who owns it.
            await _clubAccess.EnsureCanManageMatchAsync(matchFormationPlayerPosition.MatchId, matchFormationPlayerPosition.LeagueTeamId);

            if (!_currentUser.IsOperator && (request.MatchId != matchFormationPlayerPosition.MatchId || request.LeagueTeamId != matchFormationPlayerPosition.LeagueTeamId))
            {
                throw new ForbiddenAccessException("Only an admin can move a lineup slot to another match or club.");
            }

            // Re-snapshotted on every update: the request may move a different player into the slot, and the copied values follow it.
            var snapshot = await _snapshotResolver.ResolveAsync(request.MatchId, request.LeagueTeamId, request.PlayerPositionId);

            await _rules.EnsureValidAsync(request.MatchId, request.LeagueTeamId, request.PlayerPositionId, snapshot.IsSuspended, matchFormationPlayerPosition.Id);

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
