using MediatR;

namespace SoccerManager.Application.Queries.LeagueTeamPlayerLoan.GetLeagueTeamPlayerLoanById;

/// <summary>
/// Represents a request to retrieve a single loan by identifier.
/// </summary>
public sealed record GetLeagueTeamPlayerLoanByIdRequest : IRequest<GetLeagueTeamPlayerLoanByIdResponse>
{
    /// <summary>
    /// The identifier of the loan to retrieve.
    /// </summary>
    public int Id { get; init; }
}
