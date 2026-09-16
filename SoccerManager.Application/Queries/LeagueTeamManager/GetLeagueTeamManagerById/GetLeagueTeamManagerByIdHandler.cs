using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamManager.GetLeagueTeamManagerById;

/// <summary>
/// Handles <see cref="GetLeagueTeamManagerByIdRequest"/> queries.
/// </summary>
public class GetLeagueTeamManagerByIdHandler : IRequestHandler<GetLeagueTeamManagerByIdRequest, GetLeagueTeamManagerByIdResponse>
{
    private readonly ILeagueTeamManagerRepository _leagueTeamManagerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamManagerByIdHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamManagerRepository">The repository used to load league team managers.</param>
    public GetLeagueTeamManagerByIdHandler(ILeagueTeamManagerRepository leagueTeamManagerRepository)
    {
        _leagueTeamManagerRepository = leagueTeamManagerRepository;
    }

    /// <summary>
    /// Retrieves the league team manager appointment identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested league team manager appointment.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team manager with the given identifier exists.</exception>
    public async Task<GetLeagueTeamManagerByIdResponse> Handle(GetLeagueTeamManagerByIdRequest request, CancellationToken cancellationToken)
    {
        var leagueTeamManager = await _leagueTeamManagerRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"LeagueTeamManager {request.Id} not found.");

        return new GetLeagueTeamManagerByIdResponse { Data = GetLeagueTeamManagerByIdMapper.ToDto(leagueTeamManager) };
    }
}
