using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerLoan.GetLeagueTeamPlayerLoanById;

/// <summary>
/// Represents the result of a <see cref="GetLeagueTeamPlayerLoanByIdRequest"/> query.
/// </summary>
public sealed record GetLeagueTeamPlayerLoanByIdResponse
{
    /// <summary>
    /// The requested loan.
    /// </summary>
    public required LeagueTeamPlayerLoanDto Data { get; init; }
}
