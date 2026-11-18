using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeaguePlayerBids;

/// <summary>
/// Handles <see cref="GetLeaguePlayerBidsRequest"/> queries.
/// </summary>
public class GetLeaguePlayerBidsHandler : IRequestHandler<GetLeaguePlayerBidsRequest, GetLeaguePlayerBidsResponse>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly IClubAccess _clubAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeaguePlayerBidsHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load the pending bids on the player.</param>
    /// <param name="clubAccess">The check that the caller may manage the club.</param>
    public GetLeaguePlayerBidsHandler(ILeagueTeamRepository leagueTeamRepository, IClubAccess clubAccess)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _clubAccess = clubAccess;
    }

    /// <summary>
    /// Retrieves the pending transfer and loan bids on the requested player of the league team, each newest first. Only bids
    /// on the player's squad registration at that club are listed.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the player's pending transfer and loan bids, both lists empty when there are none.</returns>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the player is not registered at the club.</exception>
    public async Task<GetLeaguePlayerBidsResponse> Handle(GetLeaguePlayerBidsRequest request, CancellationToken cancellationToken)
    {
        // The access check comes first: an unknown league team has no current manager, so it is a 403 for anyone but an
        // operator, who gets the 404 below.
        await _clubAccess.EnsureCanManageClubAsync(request.LeagueTeamId);

        var (isRegistered, transferBids, swapPlayers, loanBids) = await _leagueTeamRepository.GetLeaguePlayerBidsAsync(request.LeagueTeamId, request.PlayerId);

        if (!isRegistered)
        {
            throw new KeyNotFoundException($"Player {request.PlayerId} is not registered at this club.");
        }

        return new GetLeaguePlayerBidsResponse { Data = GetLeaguePlayerBidsMapper.ToDto(transferBids, swapPlayers, loanBids) };
    }
}
