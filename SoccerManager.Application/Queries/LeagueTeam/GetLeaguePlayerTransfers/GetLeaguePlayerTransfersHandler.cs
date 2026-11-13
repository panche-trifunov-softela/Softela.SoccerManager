using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerTransfers;

/// <summary>
/// Handles <see cref="GetLeaguePlayerTransfersRequest"/> queries.
/// </summary>
public class GetLeaguePlayerTransfersHandler : IRequestHandler<GetLeaguePlayerTransfersRequest, GetLeaguePlayerTransfersResponse>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly IClubAccess _clubAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeaguePlayerTransfersHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load the player's transfers and loans.</param>
    /// <param name="clubAccess">The check that the caller may manage the club.</param>
    public GetLeaguePlayerTransfersHandler(ILeagueTeamRepository leagueTeamRepository, IClubAccess clubAccess)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _clubAccess = clubAccess;
    }

    /// <summary>
    /// Retrieves the requested player's accepted transfers and loans in the league of the league team, including the
    /// transfers he was swapped in, newest season first. The player may be registered at any club of that league, rival
    /// clubs included.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the player's transfers and loans, empty when there are none.</returns>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the player is not registered at any club of the league.</exception>
    public async Task<GetLeaguePlayerTransfersResponse> Handle(GetLeaguePlayerTransfersRequest request, CancellationToken cancellationToken)
    {
        // The access check comes first: an unknown league team has no current manager, so it is a 403 for anyone but an
        // operator, who gets the 404 below.
        await _clubAccess.EnsureCanManageClubAsync(request.LeagueTeamId);

        var (isRegistered, transfers, swapPlayers) = await _leagueTeamRepository.GetLeaguePlayerTransfersAsync(request.LeagueTeamId, request.PlayerId);

        if (!isRegistered)
        {
            throw new KeyNotFoundException($"Player {request.PlayerId} is not registered at any club of this league.");
        }

        return new GetLeaguePlayerTransfersResponse { Data = GetLeaguePlayerTransfersMapper.ToDtos(transfers, swapPlayers) };
    }
}
