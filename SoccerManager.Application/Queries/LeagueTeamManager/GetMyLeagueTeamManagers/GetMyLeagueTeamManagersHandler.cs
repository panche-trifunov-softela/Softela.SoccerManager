using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamManager.GetMyLeagueTeamManagers;

/// <summary>
/// Handles <see cref="GetMyLeagueTeamManagersRequest"/> queries.
/// </summary>
public class GetMyLeagueTeamManagersHandler : IRequestHandler<GetMyLeagueTeamManagersRequest, GetMyLeagueTeamManagersResponse>
{
    private readonly ILeagueTeamManagerRepository _leagueTeamManagerRepository;
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyLeagueTeamManagersHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamManagerRepository">The repository used to load league team managers.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    public GetMyLeagueTeamManagersHandler(ILeagueTeamManagerRepository leagueTeamManagerRepository, ICurrentUser currentUser)
    {
        _leagueTeamManagerRepository = leagueTeamManagerRepository;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Retrieves every manager appointment belonging to the currently authenticated user.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching league team manager appointments.</returns>
    public async Task<GetMyLeagueTeamManagersResponse> Handle(GetMyLeagueTeamManagersRequest request, CancellationToken cancellationToken)
    {
        // A caller with no Managers row simply gets no rows back here - being a
        // non-manager is a normal state, not an error, so this never throws for it.
        var rows = await _leagueTeamManagerRepository.GetByUserIdAsync(_currentUser.UserId, request.CurrentOnly);

        return new GetMyLeagueTeamManagersResponse { Data = rows.Select(GetMyLeagueTeamManagersMapper.ToDto).ToList() };
    }
}
