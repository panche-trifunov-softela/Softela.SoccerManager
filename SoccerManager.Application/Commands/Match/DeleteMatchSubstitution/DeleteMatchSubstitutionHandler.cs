using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Dtos;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Commands.Match.DeleteMatchSubstitution;

/// <summary>
/// Handles <see cref="DeleteMatchSubstitutionRequest"/> commands. This performs a hard delete, as the substitution tables carry
/// no soft-delete flag.
/// </summary>
public class DeleteMatchSubstitutionHandler : IRequestHandler<DeleteMatchSubstitutionRequest, IReadOnlyList<MatchSubstitutionDto>>
{
    private readonly IClubAccess _clubAccess;
    private readonly IMatchLineupService _lineupService;
    private readonly IMatchSubstitutionService _substitutionService;
    private readonly IMatchFormationPlayerPositionSubstitutionRepository _matchFormationPlayerPositionSubstitutionRepository;
    private readonly IMatchTeamTacticSubstitutionRepository _matchTeamTacticSubstitutionRepository;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteMatchSubstitutionHandler"/> class.
    /// </summary>
    /// <param name="clubAccess">The check that the caller may change the club's substitutions for the match.</param>
    /// <param name="lineupService">The service that checks that the club plays the match.</param>
    /// <param name="substitutionService">The service that builds the list of substitutions left.</param>
    /// <param name="matchFormationPlayerPositionSubstitutionRepository">The repository used to delete the substitution's lineup.</param>
    /// <param name="matchTeamTacticSubstitutionRepository">The repository used to delete the substitution's instructions.</param>
    /// <param name="unitOfWork">The unit of work used to group the deletes into one transaction.</param>
    public DeleteMatchSubstitutionHandler(IClubAccess clubAccess, IMatchLineupService lineupService, IMatchSubstitutionService substitutionService, IMatchFormationPlayerPositionSubstitutionRepository matchFormationPlayerPositionSubstitutionRepository, IMatchTeamTacticSubstitutionRepository matchTeamTacticSubstitutionRepository, IUnitOfWork unitOfWork)
    {
        _clubAccess = clubAccess;
        _lineupService = lineupService;
        _substitutionService = substitutionService;
        _matchFormationPlayerPositionSubstitutionRepository = matchFormationPlayerPositionSubstitutionRepository;
        _matchTeamTacticSubstitutionRepository = matchTeamTacticSubstitutionRepository;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes the club's substitution for the match at the minute and situation, its lineup and its instructions, in one
    /// transaction. Deleting a substitution that does not exist is not an error.
    /// </summary>
    /// <param name="request">The delete request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The club's substitutions for the match after the delete.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match, or a non-operator caller's match has already started.</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    public async Task<IReadOnlyList<MatchSubstitutionDto>> Handle(DeleteMatchSubstitutionRequest request, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _clubAccess.EnsureCanManageMatchAsync(request.MatchId, request.LeagueTeamId);

            // Operators pass every match check in ClubAccess, so whether the club plays the match is checked again for every caller.
            await _lineupService.EnsureClubPlaysMatchAsync(request.MatchId, request.LeagueTeamId);

            await _matchFormationPlayerPositionSubstitutionRepository.DeleteByMinuteAndSituationAsync(request.MatchId, request.LeagueTeamId, request.MinuteAt, request.Situation);

            await _matchTeamTacticSubstitutionRepository.DeleteAsync(request.MatchId, request.LeagueTeamId, request.MinuteAt, request.Situation);

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
