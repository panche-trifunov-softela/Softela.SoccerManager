using FluentValidation;
using FluentValidation.Results;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Dtos;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.Match.SaveMatchInstructions;

/// <summary>
/// Handles <see cref="SaveMatchInstructionsRequest"/> commands.
/// </summary>
public class SaveMatchInstructionsHandler : IRequestHandler<SaveMatchInstructionsRequest, MatchInstructionsDto>
{
    private readonly IClubAccess _clubAccess;
    private readonly IMatchLineupService _lineupService;
    private readonly IMatchTeamTacticRepository _matchTeamTacticRepository;
    private readonly IFormationPositionRepository _formationPositionRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveMatchInstructionsHandler"/> class.
    /// </summary>
    /// <param name="clubAccess">The check that the caller may change the club's instructions for the match.</param>
    /// <param name="lineupService">The service that reads the club's lineup, from which a new tactic's roles are picked, and its squad, which gives their ratings.</param>
    /// <param name="matchTeamTacticRepository">The repository used to read, update and create the club's tactic for the match.</param>
    /// <param name="formationPositionRepository">The repository used to load the lineup's formation slots, which tell the goalkeeper slots.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the writes into one transaction.</param>
    public SaveMatchInstructionsHandler(IClubAccess clubAccess, IMatchLineupService lineupService, IMatchTeamTacticRepository matchTeamTacticRepository, IFormationPositionRepository formationPositionRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _clubAccess = clubAccess;
        _lineupService = lineupService;
        _matchTeamTacticRepository = matchTeamTacticRepository;
        _formationPositionRepository = formationPositionRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Changes the instructions in the request, and only those, on the club's tactic for the match in one transaction. The
    /// first change needs 11 starters in the lineup: it creates the tactic from the instructions the page showed plus the
    /// change, and picks the captain and the set-piece takers the new row requires from those starters. Later changes never
    /// touch those roles and do not look at the lineup.
    /// </summary>
    /// <param name="request">The save request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The club's instructions after the save.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match, a non-operator caller's match has already started, or the first change finds fewer than 11 starters in the lineup.</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    public async Task<MatchInstructionsDto> Handle(SaveMatchInstructionsRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _clubAccess.EnsureCanManageMatchAsync(request.MatchId, request.LeagueTeamId);

            // Operators pass every match check in ClubAccess, so whether the club plays the match is checked again for every caller.
            await _lineupService.EnsureClubPlaysMatchAsync(request.MatchId, request.LeagueTeamId);

            // Updating first, in one statement that sets only the sent columns, lets two saves of different instructions both persist.
            var updated = await _matchTeamTacticRepository.UpdateInstructionsAsync(
                request.MatchId,
                request.LeagueTeamId,
                request.Mentality,
                request.Tempo,
                request.Passing,
                request.Width,
                request.Pressing,
                request.Tackling,
                request.AttackingSide,
                now,
                _currentUser.UserId);

            MatchInstructionsDto instructions;

            if (updated is not null)
            {
                instructions = SaveMatchInstructionsMapper.ToDto(request.MatchId, request.LeagueTeamId, updated);
            }
            else
            {
                instructions = await CreateTacticAsync(request, now);
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return instructions;
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    // The first change creates the row from what the page showed plus the change. The four roles are required columns, so they
    // are picked once here and this endpoint never changes them afterwards. A simultaneous first save of the same club can hit
    // the table's unique key and fail with a 500, which the page answers by reading the instructions again.
    private async Task<MatchInstructionsDto> CreateTacticAsync(SaveMatchInstructionsRequest request, DateTime now)
    {
        var shown = await _matchTeamTacticRepository.GetInstructionsAsync(request.MatchId, request.LeagueTeamId);

        var lineup = await _lineupService.GetLineupAsync(request.MatchId, request.LeagueTeamId, formationIdWhenEmpty: null);

        // The roles are picked from the starters, so a new row needs all eleven.
        var starterCount = lineup.Slots.Count(slot => slot.SlotNumber <= LineupAutoPicker.LastStarterSlot && slot.PlayerId.HasValue);

        if (starterCount < LineupAutoPicker.LastStarterSlot)
        {
            throw new ValidationException(new[] { new ValidationFailure("LeagueTeamId", "Pick 11 starters first.") });
        }

        var formationSlots = await _formationPositionRepository.GetSlotsAsync(lineup.FormationId!.Value);

        var squad = await _lineupService.GetSquadAsync(request.LeagueTeamId);

        var roles = SaveMatchInstructionsRolePicker.Pick(lineup.Slots, formationSlots, squad);

        var tactic = SaveMatchInstructionsMapper.ToDomainEntity(request, shown, roles, now, _currentUser.UserId);

        await _matchTeamTacticRepository.CreateAsync(tactic);

        return SaveMatchInstructionsMapper.ToDto(tactic);
    }
}
