using MediatR;

namespace SoccerManager.Application.Queries.Division.GetDivisions;

/// <summary>
/// Represents a request to retrieve every division belonging to a league.
/// </summary>
public sealed record GetDivisionsRequest : IRequest<GetDivisionsResponse>
{
    /// <summary>
    /// The identifier of the league whose divisions are being requested.
    /// </summary>
    public int LeagueId { get; init; }
}
