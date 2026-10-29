using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetMyLeagueTeamManagerApplications;

/// <summary>
/// Handles <see cref="GetMyLeagueTeamManagerApplicationsRequest"/> queries.
/// </summary>
public class GetMyLeagueTeamManagerApplicationsHandler : IRequestHandler<GetMyLeagueTeamManagerApplicationsRequest, GetMyLeagueTeamManagerApplicationsResponse>
{
    private readonly ILeagueTeamManagerApplicationRepository _leagueTeamManagerApplicationRepository;
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyLeagueTeamManagerApplicationsHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamManagerApplicationRepository">The repository used to load league team manager applications.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    public GetMyLeagueTeamManagerApplicationsHandler(ILeagueTeamManagerApplicationRepository leagueTeamManagerApplicationRepository, ICurrentUser currentUser)
    {
        _leagueTeamManagerApplicationRepository = leagueTeamManagerApplicationRepository;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Retrieves the currently authenticated user's pending applications that can still be accepted.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching league team manager applications.</returns>
    public async Task<GetMyLeagueTeamManagerApplicationsResponse> Handle(GetMyLeagueTeamManagerApplicationsRequest request, CancellationToken cancellationToken)
    {
        // A caller with no Managers row simply gets no rows back here - being a
        // non-manager is a normal state, not an error, so this never throws for it.
        var rows = await _leagueTeamManagerApplicationRepository.GetPendingByUserIdAsync(_currentUser.UserId);

        return new GetMyLeagueTeamManagerApplicationsResponse { Data = rows.Select(GetMyLeagueTeamManagerApplicationsMapper.ToDto).ToList() };
    }
}
