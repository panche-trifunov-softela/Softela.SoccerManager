using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerBidders;

/// <summary>
/// Handles <see cref="GetLeaguePlayerBiddersRequest"/> queries.
/// </summary>
public class GetLeaguePlayerBiddersHandler : IRequestHandler<GetLeaguePlayerBiddersRequest, GetLeaguePlayerBiddersResponse>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly IClubAccess _clubAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeaguePlayerBiddersHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load the clubs that have bid for the player.</param>
    /// <param name="clubAccess">The check that the caller may manage the club.</param>
    public GetLeaguePlayerBiddersHandler(ILeagueTeamRepository leagueTeamRepository, IClubAccess clubAccess)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _clubAccess = clubAccess;
    }

    /// <summary>
    /// Retrieves the clubs that have bid for the requested player in the league of the league team, accepted deals first,
    /// then newest first. The player may be registered at any club of that league, rival clubs included. No term of a bid is
    /// returned.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the player's bidders, empty when there are none.</returns>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the player is not registered at any club of the league.</exception>
    public async Task<GetLeaguePlayerBiddersResponse> Handle(GetLeaguePlayerBiddersRequest request, CancellationToken cancellationToken)
    {
        // The access check comes first: an unknown league team has no current manager, so it is a 403 for anyone but an
        // operator, who gets the 404 below.
        await _clubAccess.EnsureCanManageClubAsync(request.LeagueTeamId);

        var (isRegistered, bidders) = await _leagueTeamRepository.GetLeaguePlayerBiddersAsync(request.LeagueTeamId, request.PlayerId);

        if (!isRegistered)
        {
            throw new KeyNotFoundException($"Player {request.PlayerId} is not registered at any club of this league.");
        }

        return new GetLeaguePlayerBiddersResponse { Data = GetLeaguePlayerBiddersMapper.ToDtos(bidders) };
    }
}
