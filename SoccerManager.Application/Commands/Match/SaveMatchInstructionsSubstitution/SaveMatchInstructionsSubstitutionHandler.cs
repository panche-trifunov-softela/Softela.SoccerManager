using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Dtos;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.Match.SaveMatchInstructionsSubstitution;

/// <summary>
/// Handles <see cref="SaveMatchInstructionsSubstitutionRequest"/> commands.
/// </summary>
public class SaveMatchInstructionsSubstitutionHandler : IRequestHandler<SaveMatchInstructionsSubstitutionRequest, IReadOnlyList<MatchSubstitutionDto>>
{
    private readonly IClubAccess _clubAccess;
    private readonly IMatchLineupService _lineupService;
    private readonly IMatchSubstitutionService _substitutionService;
    private readonly IMatchTeamTacticSubstitutionRepository _matchTeamTacticSubstitutionRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="SaveMatchInstructionsSubstitutionHandler"/> class.
    /// </summary>
    /// <param name="clubAccess">The check that the caller may change the club's substitutions for the match.</param>
    /// <param name="lineupService">The service that checks that the club plays the match.</param>
    /// <param name="substitutionService">The service that finds or creates the substitution and builds the saved list.</param>
    /// <param name="matchTeamTacticSubstitutionRepository">The repository used to update the substitution's instructions.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="unitOfWork">The unit of work used to group the writes into one transaction.</param>
    public SaveMatchInstructionsSubstitutionHandler(IClubAccess clubAccess, IMatchLineupService lineupService, IMatchSubstitutionService substitutionService, IMatchTeamTacticSubstitutionRepository matchTeamTacticSubstitutionRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _clubAccess = clubAccess;
        _lineupService = lineupService;
        _substitutionService = substitutionService;
        _matchTeamTacticSubstitutionRepository = matchTeamTacticSubstitutionRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Changes the instructions in the request, and only those, on the club's substitution for the match at the minute and
    /// situation, in one transaction, creating the substitution from the main lineup first when it does not exist.
    /// </summary>
    /// <param name="request">The save request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The club's substitutions for the match after the save.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match, a non-operator caller's match has already started, the club already has substitutions at another minute, or a new substitution finds fewer than 11 starters in the main lineup.</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    public async Task<IReadOnlyList<MatchSubstitutionDto>> Handle(SaveMatchInstructionsSubstitutionRequest request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _clubAccess.EnsureCanManageMatchAsync(request.MatchId, request.LeagueTeamId);

            // Operators pass every match check in ClubAccess, so whether the club plays the match is checked again for every caller.
            await _lineupService.EnsureClubPlaysMatchAsync(request.MatchId, request.LeagueTeamId);

            var tactic = await _substitutionService.EnsureSubstitutionAsync(request.MatchId, request.LeagueTeamId, request.MinuteAt, request.Situation, now, _currentUser.UserId);

            await _matchTeamTacticSubstitutionRepository.UpdateInstructionsAsync(
                tactic.Id,
                request.Mentality,
                request.Tempo,
                request.Passing,
                request.Width,
                request.Pressing,
                request.Tackling,
                request.AttackingSide,
                now,
                _currentUser.UserId);

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
}
