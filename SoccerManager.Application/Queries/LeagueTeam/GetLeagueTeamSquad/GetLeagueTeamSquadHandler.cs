using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquad;

/// <summary>
/// Handles <see cref="GetLeagueTeamSquadRequest"/> queries.
/// </summary>
public class GetLeagueTeamSquadHandler : IRequestHandler<GetLeagueTeamSquadRequest, GetLeagueTeamSquadResponse>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamSquadHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load the league team's squad.</param>
    public GetLeagueTeamSquadHandler(ILeagueTeamRepository leagueTeamRepository)
    {
        _leagueTeamRepository = leagueTeamRepository;
    }

    /// <summary>
    /// Retrieves the requested league team's squad, with each player's current rating and main position.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the squad, ordered by line and position.</returns>
    public async Task<GetLeagueTeamSquadResponse> Handle(GetLeagueTeamSquadRequest request, CancellationToken cancellationToken)
    {
        // An unknown league team is an empty list, not an error: its 404 comes from GET /api/league-teams/{id}.
        var rows = await _leagueTeamRepository.GetSquadAsync(request.LeagueTeamId);

        return new GetLeagueTeamSquadResponse { Data = rows.Select(GetLeagueTeamSquadMapper.ToDto).ToList() };
    }
}
