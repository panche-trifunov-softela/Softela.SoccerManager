using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeams;

/// <summary>
/// Handles <see cref="GetLeagueTeamsRequest"/> queries.
/// </summary>
public class GetLeagueTeamsHandler : IRequestHandler<GetLeagueTeamsRequest, GetLeagueTeamsResponse>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamsHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load league teams.</param>
    public GetLeagueTeamsHandler(ILeagueTeamRepository leagueTeamRepository)
    {
        _leagueTeamRepository = leagueTeamRepository;
    }

    /// <summary>
    /// Retrieves every team on the roster of the requested league.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching league teams.</returns>
    public async Task<GetLeagueTeamsResponse> Handle(GetLeagueTeamsRequest request, CancellationToken cancellationToken)
    {
        // No league-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps ILeagueRepository out of a
        // handler that only needs ILeagueTeamRepository.
        var leagueTeams = await _leagueTeamRepository.GetByLeagueIdAsync(request.LeagueId);

        return new GetLeagueTeamsResponse { Data = leagueTeams.Select(GetLeagueTeamsMapper.ToDto).ToList() };
    }
}
