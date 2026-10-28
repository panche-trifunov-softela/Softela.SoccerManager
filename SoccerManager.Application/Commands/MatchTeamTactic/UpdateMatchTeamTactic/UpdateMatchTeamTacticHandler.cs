using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.MatchTeamTactic.UpdateMatchTeamTactic;

/// <summary>
/// Handles <see cref="UpdateMatchTeamTacticRequest"/> commands.
/// </summary>
public class UpdateMatchTeamTacticHandler : IRequestHandler<UpdateMatchTeamTacticRequest, bool>
{
    private readonly IMatchTeamTacticRepository _matchTeamTacticRepository;
    private readonly IClubAccess _clubAccess;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateMatchTeamTacticHandler"/> class.
    /// </summary>
    /// <param name="matchTeamTacticRepository">The repository used to load and persist match team tactics.</param>
    /// <param name="clubAccess">The check that the caller may change the club's tactics for the match.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the write into one transaction.</param>
    public UpdateMatchTeamTacticHandler(IMatchTeamTacticRepository matchTeamTacticRepository, IClubAccess clubAccess, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _matchTeamTacticRepository = matchTeamTacticRepository;
        _clubAccess = clubAccess;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Updates an existing match team tactic with the values from the given request.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns><see langword="true"/> when the update succeeds.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no match team tactic with the given identifier exists, or when the match does not exist.</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager, or when a non-operator caller moves the tactic to another match or club.</exception>
    /// <exception cref="ValidationException">Thrown when a non-operator caller's club does not play in the match or the match has already started.</exception>
    public async Task<bool> Handle(UpdateMatchTeamTacticRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var matchTeamTactic = await _matchTeamTacticRepository.GetByIdAsync(request.Id)
                ?? throw new KeyNotFoundException($"Match team tactic {request.Id} not found.");

            // The row's own match and club are checked, not the request's: the stored values decide who owns it.
            await _clubAccess.EnsureCanManageMatchAsync(matchTeamTactic.MatchId, matchTeamTactic.LeagueTeamId);

            if (!_currentUser.IsOperator && (request.MatchId != matchTeamTactic.MatchId || request.LeagueTeamId != matchTeamTactic.LeagueTeamId))
            {
                throw new ForbiddenAccessException("Only an admin can move a tactic to another match or club.");
            }

            var now = DateTime.UtcNow;

            UpdateMatchTeamTacticMapper.ApplyTo(request, matchTeamTactic, now, _currentUser.UserId);

            await _matchTeamTacticRepository.UpdateAsync(matchTeamTactic);

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
