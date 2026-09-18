using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamPlayer.GetLeagueTeamPlayerById;

/// <summary>
/// Handles <see cref="GetLeagueTeamPlayerByIdRequest"/> queries.
/// </summary>
public class GetLeagueTeamPlayerByIdHandler : IRequestHandler<GetLeagueTeamPlayerByIdRequest, GetLeagueTeamPlayerByIdResponse>
{
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayerByIdHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerRepository">The repository used to load league team players.</param>
    public GetLeagueTeamPlayerByIdHandler(ILeagueTeamPlayerRepository leagueTeamPlayerRepository)
    {
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
    }

    /// <summary>
    /// Retrieves the league team player identified by the given request.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the requested league team player.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when no league team player with the given identifier exists.</exception>
    public async Task<GetLeagueTeamPlayerByIdResponse> Handle(GetLeagueTeamPlayerByIdRequest request, CancellationToken cancellationToken)
    {
        var leagueTeamPlayer = await _leagueTeamPlayerRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException($"League team player {request.Id} not found.");

        return new GetLeagueTeamPlayerByIdResponse { Data = GetLeagueTeamPlayerByIdMapper.ToDto(leagueTeamPlayer) };
    }
}
