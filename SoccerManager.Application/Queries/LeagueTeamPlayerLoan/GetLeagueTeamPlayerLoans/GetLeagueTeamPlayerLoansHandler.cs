using MediatR;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerLoan.GetLeagueTeamPlayerLoans;

/// <summary>
/// Handles <see cref="GetLeagueTeamPlayerLoansRequest"/> queries.
/// </summary>
public class GetLeagueTeamPlayerLoansHandler : IRequestHandler<GetLeagueTeamPlayerLoansRequest, GetLeagueTeamPlayerLoansResponse>
{
    private readonly ILeagueTeamPlayerLoanRepository _leagueTeamPlayerLoanRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetLeagueTeamPlayerLoansHandler"/> class.
    /// </summary>
    /// <param name="leagueTeamPlayerLoanRepository">The repository used to load loans.</param>
    public GetLeagueTeamPlayerLoansHandler(ILeagueTeamPlayerLoanRepository leagueTeamPlayerLoanRepository)
    {
        _leagueTeamPlayerLoanRepository = leagueTeamPlayerLoanRepository;
    }

    /// <summary>
    /// Retrieves every loan of the requested squad row.
    /// </summary>
    /// <param name="request">The query request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The response carrying the matching loans.</returns>
    public async Task<GetLeagueTeamPlayerLoansResponse> Handle(GetLeagueTeamPlayerLoansRequest request, CancellationToken cancellationToken)
    {
        // No squad-row existence check here: a filter matching nothing is not an
        // error, just an empty list, and this keeps ILeagueTeamPlayerRepository out of a
        // handler that only needs ILeagueTeamPlayerLoanRepository.
        var leagueTeamPlayerLoans = await _leagueTeamPlayerLoanRepository.GetByLeagueTeamPlayerIdAsync(request.LeagueTeamPlayerId);

        return new GetLeagueTeamPlayerLoansResponse { Data = leagueTeamPlayerLoans.Select(GetLeagueTeamPlayerLoansMapper.ToDto).ToList() };
    }
}
