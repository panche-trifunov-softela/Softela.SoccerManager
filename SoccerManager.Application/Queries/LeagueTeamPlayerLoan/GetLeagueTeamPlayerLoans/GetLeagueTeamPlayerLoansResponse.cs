using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerLoan.GetLeagueTeamPlayerLoans;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamPlayerLoansRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamPlayerLoansResponse
{
    /// <summary>
    /// The list of loans of the requested squad row.
    /// </summary>
    public required List<LeagueTeamPlayerLoanDto> Data { get; init; }
}
