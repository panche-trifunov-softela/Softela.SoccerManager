using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeam.GetAvailableLeagueTeams;

/// <summary>
/// Handles <see cref="GetAvailableLeagueTeamsRequest"/> queries.
/// </summary>
public class GetAvailableLeagueTeamsHandler : IRequestHandler<GetAvailableLeagueTeamsRequest, GetAvailableLeagueTeamsResponse>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAvailableLeagueTeamsHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load the league's clubs.</param>
    public GetAvailableLeagueTeamsHandler(ILeagueTeamRepository leagueTeamRepository)
    {
        _leagueTeamRepository = leagueTeamRepository;
    }

    /// <summary>
    /// Retrieves every club on the requested league's roster, with whether each one has a current manager.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the league's clubs, by team name.</returns>
    public async Task<GetAvailableLeagueTeamsResponse> Handle(GetAvailableLeagueTeamsRequest request, CancellationToken cancellationToken)
    {
        // An unknown league is an empty list, not an error: the league's 404 comes from GET /api/leagues/{id}.
        var rows = await _leagueTeamRepository.GetAvailableByLeagueIdAsync(request.LeagueId);

        return new GetAvailableLeagueTeamsResponse { Data = rows.Select(GetAvailableLeagueTeamsMapper.ToDto).ToList() };
    }
}
