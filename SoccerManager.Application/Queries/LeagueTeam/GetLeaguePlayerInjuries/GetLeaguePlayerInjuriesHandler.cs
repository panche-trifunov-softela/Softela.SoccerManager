using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerInjuries;

/// <summary>
/// Handles <see cref="GetLeaguePlayerInjuriesRequest"/> queries.
/// </summary>
public class GetLeaguePlayerInjuriesHandler : IRequestHandler<GetLeaguePlayerInjuriesRequest, GetLeaguePlayerInjuriesResponse>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly IClubAccess _clubAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeaguePlayerInjuriesHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load the player's injuries.</param>
    /// <param name="clubAccess">The check that the caller may manage the club.</param>
    public GetLeaguePlayerInjuriesHandler(ILeagueTeamRepository leagueTeamRepository, IClubAccess clubAccess)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _clubAccess = clubAccess;
    }

    /// <summary>
    /// Retrieves the requested player's injuries recorded on the player's registration at a club of the league of the
    /// league team, newest first. The player may be registered at any club of that league, rival clubs included.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the player's injuries, empty when there are none.</returns>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the player is not registered at any club of the league.</exception>
    public async Task<GetLeaguePlayerInjuriesResponse> Handle(GetLeaguePlayerInjuriesRequest request, CancellationToken cancellationToken)
    {
        // The access check comes first: an unknown league team has no current manager, so it is a 403 for anyone but an
        // operator, who gets the 404 below.
        await _clubAccess.EnsureCanManageClubAsync(request.LeagueTeamId);

        var (isRegistered, injuries) = await _leagueTeamRepository.GetLeaguePlayerInjuriesAsync(request.LeagueTeamId, request.PlayerId);

        if (!isRegistered)
        {
            throw new KeyNotFoundException($"Player {request.PlayerId} is not registered at any club of this league.");
        }

        return new GetLeaguePlayerInjuriesResponse { Data = injuries.Select(GetLeaguePlayerInjuriesMapper.ToDto).ToList() };
    }
}
