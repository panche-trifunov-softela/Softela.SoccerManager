using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetLeagueTeamManagerApplications;

/// <summary>
/// Handles <see cref="GetLeagueTeamManagerApplicationsRequest"/> queries.
/// </summary>
public class GetLeagueTeamManagerApplicationsHandler : IRequestHandler<GetLeagueTeamManagerApplicationsRequest, GetLeagueTeamManagerApplicationsResponse>
{
    private readonly ILeagueTeamManagerApplicationRepository _applicationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamManagerApplicationsHandler"/> class.
    /// </summary>
    /// <param name="applicationRepository">The repository used to load league team manager applications.</param>
    public GetLeagueTeamManagerApplicationsHandler(ILeagueTeamManagerApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    /// <summary>
    /// Retrieves every application belonging to the requested league and team, newest first.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching league team manager applications.</returns>
    public async Task<GetLeagueTeamManagerApplicationsResponse> Handle(GetLeagueTeamManagerApplicationsRequest request, CancellationToken cancellationToken)
    {
        // No league/team-existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps ILeagueRepository and ITeamRepository
        // out of a handler that only needs ILeagueTeamManagerApplicationRepository.
        var applications = await _applicationRepository.GetByLeagueAndTeamAsync(request.LeagueId, request.TeamId);

        return new GetLeagueTeamManagerApplicationsResponse { Data = applications.Select(GetLeagueTeamManagerApplicationsMapper.ToDto).ToList() };
    }
}
