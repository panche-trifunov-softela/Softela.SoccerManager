using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Commands.Match.SaveMatchLineup;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Commands.Match.SaveMatchLineupSubstitution;

/// <summary>
/// Handles <see cref="SaveMatchLineupSubstitutionRequest"/> commands.
/// </summary>
public class SaveMatchLineupSubstitutionHandler : IRequestHandler<SaveMatchLineupSubstitutionRequest, IReadOnlyList<MatchSubstitutionDto>>
{
    private readonly IClubAccess _clubAccess;
    private readonly IMatchLineupService _lineupService;
    private readonly IMatchSubstitutionService _substitutionService;
    private readonly IMatchFormationPlayerPositionSubstitutionRepository _matchFormationPlayerPositionSubstitutionRepository;
    private readonly IMatchTeamTacticSubstitutionRepository _matchTeamTacticSubstitutionRepository;
    private readonly IFormationPositionRepository _formationPositionRepository;
    private readonly ILineupAutoPicker _picker;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveMatchLineupSubstitutionHandler"/> class.
    /// </summary>
    /// <param name="clubAccess">The check that the caller may change the club's substitutions for the match.</param>
    /// <param name="lineupService">The service that reads the club's squad and checks that the club plays the match.</param>
    /// <param name="substitutionService">The service that finds or creates the substitution and builds the saved list.</param>
    /// <param name="matchFormationPlayerPositionSubstitutionRepository">The repository used to read the substitution's lineup rows, delete the ones a save replaces and create the new ones.</param>
    /// <param name="matchTeamTacticSubstitutionRepository">The repository used to move the substitution's roles.</param>
    /// <param name="formationPositionRepository">The repository used to load the target formation's slots.</param>
    /// <param name="picker">The picker that re-seats the starters when the formation changes.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the writes into one transaction.</param>
    public SaveMatchLineupSubstitutionHandler(IClubAccess clubAccess, IMatchLineupService lineupService, IMatchSubstitutionService substitutionService, IMatchFormationPlayerPositionSubstitutionRepository matchFormationPlayerPositionSubstitutionRepository, IMatchTeamTacticSubstitutionRepository matchTeamTacticSubstitutionRepository, IFormationPositionRepository formationPositionRepository, ILineupAutoPicker picker, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _clubAccess = clubAccess;
        _lineupService = lineupService;
        _substitutionService = substitutionService;
        _matchFormationPlayerPositionSubstitutionRepository = matchFormationPlayerPositionSubstitutionRepository;
        _matchTeamTacticSubstitutionRepository = matchTeamTacticSubstitutionRepository;
        _formationPositionRepository = formationPositionRepository;
        _picker = picker;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Applies the request's formation change, slot change, or both to the club's substitution for the match at the minute and
    /// situation, in one transaction, creating the substitution from the main lineup first when it does not exist. A role player
    /// taken off hands the role to the player brought on.
    /// </summary>
    /// <param name="request">The save request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The club's substitutions for the match after the save.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the match or the formation does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match, a non-operator caller's match has already started, the club already has substitutions at another minute, or the change breaks a substitution rule.</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    public async Task<IReadOnlyList<MatchSubstitutionDto>> Handle(SaveMatchLineupSubstitutionRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _clubAccess.EnsureCanManageMatchAsync(request.MatchId, request.LeagueTeamId);

            // Operators pass every match check in ClubAccess, so whether the club plays the match is checked again for every caller.
            await _lineupService.EnsureClubPlaysMatchAsync(request.MatchId, request.LeagueTeamId);

            var tactic = await _substitutionService.EnsureSubstitutionAsync(request.MatchId, request.LeagueTeamId, request.MinuteAt, request.Situation, now, _currentUser.UserId);

            var clubRows = await _matchFormationPlayerPositionSubstitutionRepository.GetLineupRowsAsync(request.MatchId, request.LeagueTeamId);

            var rows = clubRows
                .Where(row => row.MinuteAt == request.MinuteAt && row.Situation == request.Situation)
                .ToList<GetMatchLineupRowsResult>();

            // The rows come in slot order, so the first one names the substitution's formation.
            var storedFormationId = rows.Count > 0 ? rows[0].FormationId : (int?)null;

            var formationId = request.FormationId ?? storedFormationId
                ?? throw new ValidationException(new[] { new ValidationFailure("FormationId", "Choose a formation first.") });

            var slots = await _formationPositionRepository.GetSlotsAsync(formationId);

            if (slots.Count == 0)
            {
                throw new KeyNotFoundException($"Formation {formationId} not found.");
            }

            var squad = await _lineupService.GetSquadAsync(request.LeagueTeamId);

            var mainRows = await _substitutionService.GetMainLineupRowsAsync(request.MatchId, request.LeagueTeamId);

            var before = SaveMatchLineupSubstitutionRules.SeatsOf(rows, storedFormationId);

            var planSquad = SaveMatchLineupSubstitutionRules.ToPlanSquad(squad, before.Values);

            // The main lineup's planner applies the change, so a substitution's lineup follows the same slot rules.
            var lineupRequest = new SaveMatchLineupRequest
            {
                MatchId = request.MatchId,
                LeagueTeamId = request.LeagueTeamId,
                FormationId = request.FormationId,
                SlotNumber = request.SlotNumber,
                PlayerId = request.PlayerId,
            };

            var (deleteRowIds, inserts) = SaveMatchLineupPlanner.Plan(lineupRequest, rows, storedFormationId, formationId, slots, planSquad, _picker);

            var after = SaveMatchLineupSubstitutionRules.SeatsAfter(rows, formationId, deleteRowIds, inserts);

            SaveMatchLineupSubstitutionRules.EnsureAllowed(before, after, mainRows, squad);

            foreach (var id in deleteRowIds)
            {
                await _matchFormationPlayerPositionSubstitutionRepository.DeleteAsync(id);
            }

            await InsertAsync(request, slots, squad, inserts, now);

            var roles = SaveMatchLineupSubstitutionRules.MoveRoles(tactic, before, after);

            if (roles != (tactic.CaptainPlayerId, tactic.PenaltyTakerPlayerId, tactic.FreeKickTakerPlayerId, tactic.CornerKickTakerPlayerId))
            {
                await _matchTeamTacticSubstitutionRepository.UpdateRolesAsync(tactic.Id, roles.PenaltyTakerPlayerId, roles.FreeKickTakerPlayerId, roles.CornerKickTakerPlayerId, roles.CaptainPlayerId, now, _currentUser.UserId);
            }

            var substitutions = await _substitutionService.GetSubstitutionsAsync(request.MatchId, request.LeagueTeamId);

            await _unitOfWork.CommitAsync(cancellationToken);

            return substitutions;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    // The snapshots come from the true squad, so a suspended player taken off is stored as suspended.
    private async Task InsertAsync(SaveMatchLineupSubstitutionRequest request, IReadOnlyList<GetFormationSlotsResult> slots, IReadOnlyDictionary<int, LineupCandidate> squad, IReadOnlyDictionary<int, int> assignments, DateTime now)
    {
        foreach (var (slotNumber, playerId) in assignments.OrderBy(assignment => assignment.Key))
        {
            var slot = slots.First(formationSlot => formationSlot.SlotNumber == slotNumber);

            var row = MatchLineupSnapshot.ToRow<MatchFormationPlayerPositionSubstitution>(request.MatchId, request.LeagueTeamId, slot, squad[playerId], now, _currentUser.UserId);
            row.MinuteAt = request.MinuteAt;
            row.Situation = request.Situation;

            await _matchFormationPlayerPositionSubstitutionRepository.CreateAsync(row);
        }
    }
}
