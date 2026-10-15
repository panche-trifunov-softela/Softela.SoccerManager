using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeam.GetAvailableLeagueTeams;

/// <summary>
/// Handles <see cref="GetAvailableLeagueTeamsRequest"/> queries.
/// </summary>
public class GetAvailableLeagueTeamsHandler : IRequestHandler<GetAvailableLeagueTeamsRequest, GetAvailableLeagueTeamsResponse>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAvailableLeagueTeamsHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load the league's clubs.</param>
    /// <param name="currentUser">The currently authenticated user, whose pending applications are flagged.</param>
    public GetAvailableLeagueTeamsHandler(ILeagueTeamRepository leagueTeamRepository, ICurrentUser currentUser)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Retrieves every club on the requested league's roster, with whether each one has a current manager and whether the caller
    /// has a pending application for it.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the league's clubs, by team name.</returns>
    public async Task<GetAvailableLeagueTeamsResponse> Handle(GetAvailableLeagueTeamsRequest request, CancellationToken cancellationToken)
    {
        // An unknown league is an empty list, not an error: the league's 404 comes from GET /api/leagues/{id}.
        var rows = await _leagueTeamRepository.GetAvailableByLeagueIdAsync(request.LeagueId, _currentUser.UserId);

        return new GetAvailableLeagueTeamsResponse { Data = rows.Select(GetAvailableLeagueTeamsMapper.ToDto).ToList() };
    }
}
