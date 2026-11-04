using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Dtos;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.Match.SaveMatchLineup;

/// <summary>
/// Handles <see cref="SaveMatchLineupRequest"/> commands.
/// </summary>
public class SaveMatchLineupHandler : IRequestHandler<SaveMatchLineupRequest, MatchLineupDto>
{
    private readonly IClubAccess _clubAccess;
    private readonly IMatchLineupService _lineupService;
    private readonly IMatchFormationPlayerPositionRepository _matchFormationPlayerPositionRepository;
    private readonly IFormationPositionRepository _formationPositionRepository;
    private readonly ILineupAutoPicker _picker;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveMatchLineupHandler"/> class.
    /// </summary>
    /// <param name="clubAccess">The check that the caller may change the club's lineup for the match.</param>
    /// <param name="lineupService">The service that reads and writes the club's lineup.</param>
    /// <param name="matchFormationPlayerPositionRepository">The repository used to read the club's lineup rows and delete the ones a save replaces.</param>
    /// <param name="formationPositionRepository">The repository used to load the target formation's slots.</param>
    /// <param name="picker">The picker that re-seats the starters when the formation changes.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the writes into one transaction.</param>
    public SaveMatchLineupHandler(IClubAccess clubAccess, IMatchLineupService lineupService, IMatchFormationPlayerPositionRepository matchFormationPlayerPositionRepository, IFormationPositionRepository formationPositionRepository, ILineupAutoPicker picker, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _clubAccess = clubAccess;
        _lineupService = lineupService;
        _matchFormationPlayerPositionRepository = matchFormationPlayerPositionRepository;
        _formationPositionRepository = formationPositionRepository;
        _picker = picker;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Applies the request's formation change, slot change, or both to the club's lineup for the match in one transaction,
    /// snapshotting each player written. Rows of another formation than the lineup's, which the removed per-slot endpoints may
    /// have left, are ignored, and are deleted only when the formation changes.
    /// </summary>
    /// <param name="request">The save request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The club's lineup after the save.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the match or the formation does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match, a non-operator caller's match has already started, or a slot or player breaks a lineup rule.</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    public async Task<MatchLineupDto> Handle(SaveMatchLineupRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _clubAccess.EnsureCanManageMatchAsync(request.MatchId, request.LeagueTeamId);

            // Operators pass every match check in ClubAccess, so whether the club plays the match is checked again for every caller.
            await _lineupService.EnsureClubPlaysMatchAsync(request.MatchId, request.LeagueTeamId);

            var rows = await _matchFormationPlayerPositionRepository.GetLineupRowsAsync(request.MatchId, request.LeagueTeamId);

            // The rows come in slot order, so the first one names the lineup's formation.
            var storedFormationId = rows.Count > 0 ? rows[0].FormationId : (int?)null;

            var formationId = request.FormationId ?? storedFormationId
                ?? throw new ValidationException(new[] { new ValidationFailure("FormationId", "Choose a formation first.") });

            var slots = await _formationPositionRepository.GetSlotsAsync(formationId);

            if (slots.Count == 0)
            {
                throw new KeyNotFoundException($"Formation {formationId} not found.");
            }

            var squad = await _lineupService.GetSquadAsync(request.LeagueTeamId);

            var (deleteRowIds, inserts) = SaveMatchLineupPlanner.Plan(request, rows, storedFormationId, formationId, slots, squad, _picker);

            foreach (var id in deleteRowIds)
            {
                await _matchFormationPlayerPositionRepository.DeleteAsync(id);
            }

            await _lineupService.InsertAsync(request.MatchId, request.LeagueTeamId, slots, squad, inserts, now, _currentUser.UserId);

            // An emptied lineup is returned in the target formation, since the stored rows no longer say which it was.
            var lineup = await _lineupService.GetLineupAsync(request.MatchId, request.LeagueTeamId, formationIdWhenEmpty: formationId);

            await _unitOfWork.CommitAsync(cancellationToken);

            return lineup;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
