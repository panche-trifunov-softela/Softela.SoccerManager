using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamManager.GetLeagueTeamManagers;

/// <summary>
/// Handles <see cref="GetLeagueTeamManagersRequest"/> queries.
/// </summary>
public class GetLeagueTeamManagersHandler : IRequestHandler<GetLeagueTeamManagersRequest, GetLeagueTeamManagersResponse>
{
    private readonly ILeagueTeamManagerRepository _leagueTeamManagerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamManagersHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamManagerRepository">The repository used to load league team managers.</param>
    public GetLeagueTeamManagersHandler(ILeagueTeamManagerRepository leagueTeamManagerRepository)
    {
        _leagueTeamManagerRepository = leagueTeamManagerRepository;
    }

    /// <summary>
    /// Retrieves every manager appointment belonging to the requested league roster entry.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching league team manager appointments.</returns>
    public async Task<GetLeagueTeamManagersResponse> Handle(GetLeagueTeamManagersRequest request, CancellationToken cancellationToken)
    {
        // No roster-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps ILeagueTeamRepository
        // out of a handler that only needs ILeagueTeamManagerRepository.
        var leagueTeamManagers = await _leagueTeamManagerRepository.GetByLeagueTeamIdAsync(request.LeagueTeamId);

        return new GetLeagueTeamManagersResponse { Data = leagueTeamManagers.Select(GetLeagueTeamManagersMapper.ToDto).ToList() };
    }
}
