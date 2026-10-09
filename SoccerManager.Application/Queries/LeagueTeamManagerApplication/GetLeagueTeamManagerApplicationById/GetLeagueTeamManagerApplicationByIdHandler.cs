using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetLeagueTeamManagerApplicationById;

/// <summary>
/// Handles <see cref="GetLeagueTeamManagerApplicationByIdRequest"/> queries.
/// </summary>
public class GetLeagueTeamManagerApplicationByIdHandler : IRequestHandler<GetLeagueTeamManagerApplicationByIdRequest, GetLeagueTeamManagerApplicationByIdResponse>
{
    private readonly ILeagueTeamManagerApplicationRepository _applicationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamManagerApplicationByIdHandler"/> class.
    /// </summary>
    /// <param name="applicationRepository">The repository used to load league team manager applications.</param>
    public GetLeagueTeamManagerApplicationByIdHandler(ILeagueTeamManagerApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    /// <summary>
    /// Retrieves the league team manager application identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested league team manager application.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team manager application with the given identifier exists.</exception>
    public async Task<GetLeagueTeamManagerApplicationByIdResponse> Handle(GetLeagueTeamManagerApplicationByIdRequest request, CancellationToken cancellationToken)
    {
        var application = await _applicationRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"LeagueTeamManagerApplication {request.Id} not found.");

        return new GetLeagueTeamManagerApplicationByIdResponse { Data = GetLeagueTeamManagerApplicationByIdMapper.ToDto(application) };
    }
}
