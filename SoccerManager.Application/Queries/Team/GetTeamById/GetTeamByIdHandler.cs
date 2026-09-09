using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.Team.GetTeamById;

/// <summary>
/// Handles <see cref="GetTeamByIdRequest"/> queries.
/// </summary>
public class GetTeamByIdHandler : IRequestHandler<GetTeamByIdRequest, GetTeamByIdResponse>
{
    private readonly ITeamRepository _teamRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTeamByIdHandler"/> class.
    /// </summary>
    /// <param name="teamRepository">The repository used to load teams.</param>
    public GetTeamByIdHandler(ITeamRepository teamRepository)
    {
        _teamRepository = teamRepository;
    }

    /// <summary>
    /// Retrieves the team identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested team.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no team with the given identifier exists.</exception>
    public async Task<GetTeamByIdResponse> Handle(GetTeamByIdRequest request, CancellationToken cancellationToken)
    {
        var team = await _teamRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"Team {request.Id} not found.");

        return new GetTeamByIdResponse { Data = GetTeamByIdMapper.ToDto(team) };
    }
}
