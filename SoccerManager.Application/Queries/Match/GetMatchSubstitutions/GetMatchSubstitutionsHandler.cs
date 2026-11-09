using FluentValidation;
using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Queries.Match.GetMatchSubstitutions;

/// <summary>
/// Handles <see cref="GetMatchSubstitutionsRequest"/> queries.
/// </summary>
public class GetMatchSubstitutionsHandler : IRequestHandler<GetMatchSubstitutionsRequest, GetMatchSubstitutionsResponse>
{
    private readonly IClubAccess _clubAccess;
    private readonly IMatchLineupService _lineupService;
    private readonly IMatchSubstitutionService _substitutionService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMatchSubstitutionsHandler"/> class.
    /// </summary>
    /// <param name="clubAccess">The check that the caller may manage the club.</param>
    /// <param name="lineupService">The service used to check that the club plays the match.</param>
    /// <param name="substitutionService">The service used to build the club's substitutions for the match.</param>
    public GetMatchSubstitutionsHandler(IClubAccess clubAccess, IMatchLineupService lineupService, IMatchSubstitutionService substitutionService)
    {
        _clubAccess = clubAccess;
        _lineupService = lineupService;
        _substitutionService = substitutionService;
    }

    /// <summary>
    /// Retrieves the club's saved substitutions for the match, ordered by minute, then situation. It never writes.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the club's substitutions, empty when it has none.</returns>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match.</exception>
    public async Task<GetMatchSubstitutionsResponse> Handle(GetMatchSubstitutionsRequest request, CancellationToken cancellationToken)
    {
        await _clubAccess.EnsureCanManageClubAsync(request.LeagueTeamId);

        // Operators pass ClubAccess, so whether the club plays the match is checked for every caller.
        await _lineupService.EnsureClubPlaysMatchAsync(request.MatchId, request.LeagueTeamId);

        var substitutions = await _substitutionService.GetSubstitutionsAsync(request.MatchId, request.LeagueTeamId);

        return new GetMatchSubstitutionsResponse { Data = substitutions };
    }
}
