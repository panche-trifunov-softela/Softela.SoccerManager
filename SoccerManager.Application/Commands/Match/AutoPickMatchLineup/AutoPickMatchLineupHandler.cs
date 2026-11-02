using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Dtos;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.Match.AutoPickMatchLineup;

/// <summary>
/// Handles <see cref="AutoPickMatchLineupRequest"/> commands.
/// </summary>
public class AutoPickMatchLineupHandler : IRequestHandler<AutoPickMatchLineupRequest, MatchLineupDto>
{
    private readonly IClubAccess _clubAccess;
    private readonly IMatchLineupService _lineupService;
    private readonly IMatchFormationPlayerPositionRepository _matchFormationPlayerPositionRepository;
    private readonly IFormationPositionRepository _formationPositionRepository;
    private readonly ILineupAutoPicker _picker;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoPickMatchLineupHandler"/> class.
    /// </summary>
    /// <param name="clubAccess">The check that the caller may change the club's lineup for the match.</param>
    /// <param name="lineupService">The service that reads and writes the club's lineup.</param>
    /// <param name="matchFormationPlayerPositionRepository">The repository used to read and remove the club's stored lineup.</param>
    /// <param name="formationPositionRepository">The repository used to load the formation's slots.</param>
    /// <param name="picker">The picker that chooses the lineup from the squad.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the writes into one transaction.</param>
    public AutoPickMatchLineupHandler(IClubAccess clubAccess, IMatchLineupService lineupService, IMatchFormationPlayerPositionRepository matchFormationPlayerPositionRepository, IFormationPositionRepository formationPositionRepository, ILineupAutoPicker picker, ICurrentUser currentUser, IUnitOfWork unitOfWork)
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
    /// Replaces the club's whole lineup for the match, rows of other formations included, with one picked from its squad in
    /// the requested, stored or default formation, in one transaction.
    /// </summary>
    /// <param name="request">The auto pick request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The club's lineup after the pick; the formation's slots all empty when no player is eligible.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the match or the formation does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match, a non-operator caller's match has already started, or no formation is available.</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    public async Task<MatchLineupDto> Handle(AutoPickMatchLineupRequest request, CancellationToken cancellationToken)
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

            var formationId = request.FormationId ?? storedFormationId ?? await _lineupService.GetDefaultFormationIdAsync()
                ?? throw new ValidationException(new[] { new ValidationFailure("FormationId", "No formation is available for a lineup.") });

            var slots = await _formationPositionRepository.GetSlotsAsync(formationId);

            if (slots.Count == 0)
            {
                throw new KeyNotFoundException($"Formation {formationId} not found.");
            }

            var squad = await _lineupService.GetSquadAsync(request.LeagueTeamId);

            var picked = _picker.Pick(slots, squad.Values);

            // Drops every row of the club for the match, rows of other formations included.
            await _matchFormationPlayerPositionRepository.DeleteByMatchAndLeagueTeamAsync(request.MatchId, request.LeagueTeamId);

            await _lineupService.InsertAsync(request.MatchId, request.LeagueTeamId, slots, squad, picked, now, _currentUser.UserId);

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
