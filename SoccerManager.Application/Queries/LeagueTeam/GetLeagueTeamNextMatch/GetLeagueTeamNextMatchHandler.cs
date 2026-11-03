using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamNextMatch;

/// <summary>
/// Handles <see cref="GetLeagueTeamNextMatchRequest"/> queries.
/// </summary>
public class GetLeagueTeamNextMatchHandler : IRequestHandler<GetLeagueTeamNextMatchRequest, GetLeagueTeamNextMatchResponse>
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMatchLineupService _lineupService;
    private readonly IMatchTeamTacticRepository _matchTeamTacticRepository;
    private readonly IClubAccess _clubAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamNextMatchHandler"/> class.
    /// </summary>
    /// <param name="matchRepository">The repository used to find the club's next match.</param>
    /// <param name="lineupService">The service used to build the club's lineup for the match.</param>
    /// <param name="matchTeamTacticRepository">The repository used to read the club's instructions for the match.</param>
    /// <param name="clubAccess">The check that the caller may manage the club.</param>
    public GetLeagueTeamNextMatchHandler(IMatchRepository matchRepository, IMatchLineupService lineupService, IMatchTeamTacticRepository matchTeamTacticRepository, IClubAccess clubAccess)
    {
        _matchRepository = matchRepository;
        _lineupService = lineupService;
        _matchTeamTacticRepository = matchTeamTacticRepository;
        _clubAccess = clubAccess;
    }

    /// <summary>
    /// Retrieves the club's earliest match that has not started, with its opponent and the club's lineup and instructions for it.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the next match, or no data when the club has none to come.</returns>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    public async Task<GetLeagueTeamNextMatchResponse> Handle(GetLeagueTeamNextMatchRequest request, CancellationToken cancellationToken)
    {
        // There is no existence check: an unknown league team has no current manager, so it is a 403 for anyone but an
        // operator, who gets no match.
        await _clubAccess.EnsureCanManageClubAsync(request.LeagueTeamId);

        var row = await _matchRepository.GetNextByLeagueTeamAsync(request.LeagueTeamId);

        if (row is null)
        {
            return new GetLeagueTeamNextMatchResponse { Data = null };
        }

        var lineup = await _lineupService.GetLineupAsync(row.MatchId, request.LeagueTeamId, formationIdWhenEmpty: null);

        var instructions = await _matchTeamTacticRepository.GetInstructionsAsync(row.MatchId, request.LeagueTeamId);

        return new GetLeagueTeamNextMatchResponse { Data = GetLeagueTeamNextMatchMapper.ToDto(row, lineup, instructions) };
    }
}
