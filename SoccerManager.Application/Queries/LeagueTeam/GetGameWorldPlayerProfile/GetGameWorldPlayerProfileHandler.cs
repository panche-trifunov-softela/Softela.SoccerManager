using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Queries.LeagueTeam.GetGameWorldPlayerProfile;

/// <summary>
/// Handles <see cref="GetGameWorldPlayerProfileRequest"/> queries.
/// </summary>
public class GetGameWorldPlayerProfileHandler : IRequestHandler<GetGameWorldPlayerProfileRequest, GetGameWorldPlayerProfileResponse>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly IClubAccess _clubAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetGameWorldPlayerProfileHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load the player's registration.</param>
    /// <param name="clubAccess">The check that the caller may manage the club.</param>
    public GetGameWorldPlayerProfileHandler(ILeagueTeamRepository leagueTeamRepository, IClubAccess clubAccess)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _clubAccess = clubAccess;
    }

    /// <summary>
    /// Retrieves the requested player's profile in the game world of the league team's league. The player may be registered
    /// at any club of that league, rival clubs included; the league team's own registration is preferred.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the player's profile.</returns>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the player is not registered at any club of the league.</exception>
    public async Task<GetGameWorldPlayerProfileResponse> Handle(GetGameWorldPlayerProfileRequest request, CancellationToken cancellationToken)
    {
        // The access check comes first: an unknown league team has no current manager, so it is a 403 for anyone but an
        // operator, who gets the 404 below.
        await _clubAccess.EnsureCanManageClubAsync(request.LeagueTeamId);

        var (header, positions) = await _leagueTeamRepository.GetGameWorldPlayerProfileAsync(request.LeagueTeamId, request.PlayerId);

        if (header is null)
        {
            throw new KeyNotFoundException($"Player {request.PlayerId} is not registered at any club of this game world.");
        }

        return new GetGameWorldPlayerProfileResponse { Data = GetGameWorldPlayerProfileMapper.ToDto(header, positions) };
    }
}
