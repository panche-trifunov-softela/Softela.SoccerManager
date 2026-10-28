using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.MatchFormationPlayerPosition.DeleteMatchFormationPlayerPosition;

/// <summary>
/// Handles <see cref="DeleteMatchFormationPlayerPositionRequest"/> commands. This performs a hard delete,
/// as the entity carries no soft-delete flag.
/// </summary>
public class DeleteMatchFormationPlayerPositionHandler : IRequestHandler<DeleteMatchFormationPlayerPositionRequest, bool>
{
    private readonly IMatchFormationPlayerPositionRepository _matchFormationPlayerPositionRepository;
    private readonly IClubAccess _clubAccess;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteMatchFormationPlayerPositionHandler"/> class.
    /// </summary>
    /// <param name="matchFormationPlayerPositionRepository">The repository used to load and delete match formation player positions.</param>
    /// <param name="clubAccess">The check that the caller may change the club's lineup for the match.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public DeleteMatchFormationPlayerPositionHandler(IMatchFormationPlayerPositionRepository matchFormationPlayerPositionRepository, IClubAccess clubAccess, IUnitOfWork unitOfWork)
    {
        _matchFormationPlayerPositionRepository = matchFormationPlayerPositionRepository;
        _clubAccess = clubAccess;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the match formation player position identified by the given request.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the deletion succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match formation player position with the given identifier exists, or when the match does not exist.</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    /// <exception cref="ValidationException">Thrown when a non-operator caller's club does not play in the match or the match has already started.</exception>
    public async Task<bool> Handle(DeleteMatchFormationPlayerPositionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var matchFormationPlayerPosition = await _matchFormationPlayerPositionRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Match formation player position {request.Id} not found.");

            await _clubAccess.EnsureCanManageMatchAsync(matchFormationPlayerPosition.MatchId, matchFormationPlayerPosition.LeagueTeamId);

            await _matchFormationPlayerPositionRepository.DeleteAsync(request.Id);

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
