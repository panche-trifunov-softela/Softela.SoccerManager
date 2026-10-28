using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
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
    private readonly IMatchFormationPlayerPositionRules _rules;
    private readonly IClubAccess _clubAccess;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateMatchFormationPlayerPositionHandler"/> class.
    /// </summary>
    /// <param name="matchFormationPlayerPositionRepository">The repository used to persist match formation player positions.</param>
    /// <param name="snapshotResolver">The resolver that supplies the values snapshotted from the player's registration and position rating.</param>
    /// <param name="rules">The rules a lineup slot must obey for every caller.</param>
    /// <param name="clubAccess">The check that the caller may change the club's lineup for the match.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public CreateMatchFormationPlayerPositionHandler(IMatchFormationPlayerPositionRepository matchFormationPlayerPositionRepository, IMatchFormationPlayerPositionSnapshotResolver snapshotResolver, IMatchFormationPlayerPositionRules rules, IClubAccess clubAccess, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchFormationPlayerPositionRepository = matchFormationPlayerPositionRepository;
        _snapshotResolver = snapshotResolver;
        _rules = rules;
        _clubAccess = clubAccess;
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
    /// <exception cref="ValidationException">Thrown when the player is registered with a different team in the match's league, is suspended or already has a slot in the lineup, or when a non-operator caller's club does not play in the match or the match has already started.</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    public async Task<int> Handle(CreateMatchFormationPlayerPositionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _clubAccess.EnsureCanManageMatchAsync(request.MatchId, request.LeagueTeamId);

            // Resolved inside the transaction so the snapshot and the insert see the same data.
            var snapshot = await _snapshotResolver.ResolveAsync(request.MatchId, request.LeagueTeamId, request.PlayerPositionId);

            await _rules.EnsureValidAsync(request.MatchId, request.LeagueTeamId, request.PlayerPositionId, snapshot.IsSuspended, excludedSlotId: null);

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
