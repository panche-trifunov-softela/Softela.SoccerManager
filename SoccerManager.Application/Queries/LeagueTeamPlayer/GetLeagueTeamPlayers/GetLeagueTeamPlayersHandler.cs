using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamPlayer.GetLeagueTeamPlayers;

/// <summary>
/// Handles <see cref="GetLeagueTeamPlayersRequest"/> queries.
/// </summary>
public class GetLeagueTeamPlayersHandler : IRequestHandler<GetLeagueTeamPlayersRequest, GetLeagueTeamPlayersResponse>
{
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayersHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerRepository">The repository used to load league team players.</param>
    public GetLeagueTeamPlayersHandler(ILeagueTeamPlayerRepository leagueTeamPlayerRepository)
    {
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
    }

    /// <summary>
    /// Retrieves every player registration belonging to the requested league and team.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching league team players.</returns>
    public async Task<GetLeagueTeamPlayersResponse> Handle(GetLeagueTeamPlayersRequest request, CancellationToken cancellationToken)
    {
        // No league/team-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps ILeagueRepository and ITeamRepository
        // out of a handler that only needs ILeagueTeamPlayerRepository.
        var leagueTeamPlayers = await _leagueTeamPlayerRepository.GetByLeagueAndTeamAsync(request.LeagueId, request.TeamId);

        return new GetLeagueTeamPlayersResponse { Data = leagueTeamPlayers.Select(GetLeagueTeamPlayersMapper.ToDto).ToList() };
    }
}
