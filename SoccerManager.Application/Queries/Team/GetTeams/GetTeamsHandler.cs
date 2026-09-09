using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Team.GetTeams;

/// <summary>
/// Handles <see cref="GetTeamsRequest"/> queries.
/// </summary>
public class GetTeamsHandler : IRequestHandler<GetTeamsRequest, GetTeamsResponse>
{
    private readonly ITeamRepository _teamRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTeamsHandler"/> class.
    /// </summary>
    /// <param name="teamRepository">The repository used to load teams.</param>
    public GetTeamsHandler(ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
    }

    /// <summary>
    /// Retrieves all teams.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying every team.</returns>
    public async Task<GetTeamsResponse> Handle(GetTeamsRequest request, CancellationToken cancellationToken)
    {
        var teams = await _teamRepository.GetAllAsync();

        return new GetTeamsResponse { Data = teams.Select(GetTeamsMapper.ToDto).ToList() };
    }
}
