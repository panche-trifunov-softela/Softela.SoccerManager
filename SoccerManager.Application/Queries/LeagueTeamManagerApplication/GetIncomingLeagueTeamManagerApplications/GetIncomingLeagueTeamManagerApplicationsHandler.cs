using MediatR;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetIncomingLeagueTeamManagerApplications;

/// <summary>
/// Handles <see cref="GetIncomingLeagueTeamManagerApplicationsRequest"/> queries.
/// </summary>
public class GetIncomingLeagueTeamManagerApplicationsHandler : IRequestHandler<GetIncomingLeagueTeamManagerApplicationsRequest, GetIncomingLeagueTeamManagerApplicationsResponse>
{
    private readonly ILeagueTeamManagerApplicationRepository _leagueTeamManagerApplicationRepository;
    private readonly ICurrentUser _currentUser;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetIncomingLeagueTeamManagerApplicationsHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamManagerApplicationRepository">The repository used to load league team manager applications.</param>
    /// <param name="currentUser">The currently authenticated user.</param>
    public GetIncomingLeagueTeamManagerApplicationsHandler(ILeagueTeamManagerApplicationRepository leagueTeamManagerApplicationRepository, ICurrentUser currentUser)
    {
        _leagueTeamManagerApplicationRepository = leagueTeamManagerApplicationRepository;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Retrieves the pending applications that can still be accepted for clubs in the currently authenticated user's leagues,
    /// or in every league for an operator.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching league team manager applications.</returns>
    public async Task<GetIncomingLeagueTeamManagerApplicationsResponse> Handle(GetIncomingLeagueTeamManagerApplicationsRequest request, CancellationToken cancellationToken)
    {
        // The operator flag is read from the token, never from the request, because it opens every league.
        // A non-operator who owns no league - or has no Managers row - simply gets no rows back, not an error.
        var rows = await _leagueTeamManagerApplicationRepository.GetPendingByLeagueOwnerAsync(_currentUser.UserId, _currentUser.IsOperator);

        return new GetIncomingLeagueTeamManagerApplicationsResponse { Data = rows.Select(GetIncomingLeagueTeamManagerApplicationsMapper.ToDto).ToList() };
    }
}
