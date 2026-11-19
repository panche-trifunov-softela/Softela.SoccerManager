using MediatR;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Repositories;
using SoccerManager.Application.Services;

namespace SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamLoanBids;

/// <summary>
/// Handles <see cref="GetLeagueTeamLoanBidsRequest"/> queries.
/// </summary>
public class GetLeagueTeamLoanBidsHandler : IRequestHandler<GetLeagueTeamLoanBidsRequest, GetLeagueTeamLoanBidsResponse>
{
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly IClubAccess _clubAccess;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamLoanBidsHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamRepository">The repository used to load the club's loan bids.</param>
    /// <param name="clubAccess">The check that the caller may manage the club.</param>
    public GetLeagueTeamLoanBidsHandler(ILeagueTeamRepository leagueTeamRepository, IClubAccess clubAccess)
    {
        _leagueTeamRepository = leagueTeamRepository;
        _clubAccess = clubAccess;
    }

    /// <summary>
    /// Retrieves the loan bids the club placed and received, the pending ones first, then the accepted and completed ones,
    /// each newest first. Rejected bids are not returned.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the club's pending and completed loan bids, both lists empty when there are none.</returns>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    public async Task<GetLeagueTeamLoanBidsResponse> Handle(GetLeagueTeamLoanBidsRequest request, CancellationToken cancellationToken)
    {
        // There is no existence check: an unknown league team has no current manager, so it is a 403 for anyone but an
        // operator, who gets empty lists.
        await _clubAccess.EnsureCanManageClubAsync(request.LeagueTeamId);

        var bids = await _leagueTeamRepository.GetLeagueTeamLoanBidsAsync(request.LeagueTeamId);

        return new GetLeagueTeamLoanBidsResponse { Data = GetLeagueTeamLoanBidsMapper.ToDto(bids) };
    }
}
