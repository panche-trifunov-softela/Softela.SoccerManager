using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSchedule;

/// <summary>
/// Handles <see cref="GetLeagueTeamScheduleRequest"/> queries.
/// </summary>
public class GetLeagueTeamScheduleHandler : IRequestHandler<GetLeagueTeamScheduleRequest, GetLeagueTeamScheduleResponse>
{
    private readonly IMatchRepository _matchRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamScheduleHandler"/> class.
    /// </summary>
    /// <param name="matchRepository">The repository used to load the club's matches.</param>
    public GetLeagueTeamScheduleHandler(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    /// <summary>
    /// Retrieves the requested league team's matches in its League's latest season, with the score of each finished one.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matches, earliest first.</returns>
    public async Task<GetLeagueTeamScheduleResponse> Handle(GetLeagueTeamScheduleRequest request, CancellationToken cancellationToken)
    {
        // There is no club access check: GET /api/matches already shows every match to every signed-in user, so a club's
        // schedule reveals nothing more. An unknown league team is an empty list, not an error: its 404 comes from
        // GET /api/league-teams/{id}.
        var rows = await _matchRepository.GetScheduleByLeagueTeamAsync(request.LeagueTeamId);

        return new GetLeagueTeamScheduleResponse { Data = rows.Select(GetLeagueTeamScheduleMapper.ToDto).ToList() };
    }
}
