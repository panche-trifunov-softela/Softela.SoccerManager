using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamById;

/// <summary>
/// Handles <see cref="GetLeagueTeamByIdRequest"/> queries.
/// </summary>
public class GetLeagueTeamByIdHandler : IRequestHandler<GetLeagueTeamByIdRequest, GetLeagueTeamByIdResponse>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamByIdHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load league teams.</param>
    public GetLeagueTeamByIdHandler(ILeagueTeamRepository leagueTeamRepository)
    {
        _leagueTeamRepository = leagueTeamRepository;
    }

    /// <summary>
    /// Retrieves the league team identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested league team.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team with the given identifier exists.</exception>
    public async Task<GetLeagueTeamByIdResponse> Handle(GetLeagueTeamByIdRequest request, CancellationToken cancellationToken)
    {
        var leagueTeam = await _leagueTeamRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"LeagueTeam {request.Id} not found.");

        return new GetLeagueTeamByIdResponse { Data = GetLeagueTeamByIdMapper.ToDto(leagueTeam) };
    }
}
