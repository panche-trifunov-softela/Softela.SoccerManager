using MediatR;

namespace SoccerManager.Application.Commands.LeagueTeamPlayerLoan.DeleteLeagueTeamPlayerLoan;

/// <summary>
/// Represents a request to delete an existing loan.
/// </summary>
public sealed record DeleteLeagueTeamPlayerLoanRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the loan to delete.
    /// </summary>
    public int Id { get; init; }
}
