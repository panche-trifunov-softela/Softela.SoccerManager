using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerLoan.GetLeagueTeamPlayerLoans;

/// <summary>
/// Represents a request to retrieve every loan of a squad row.
/// </summary>
public sealed record GetLeagueTeamPlayerLoansRequest : IRequest<GetLeagueTeamPlayerLoansResponse>
{
    /// <summary>
    /// The identifier of the squad row whose loans are being requested.
    /// </summary>
    public int LeagueTeamPlayerId { get; init; }
}
