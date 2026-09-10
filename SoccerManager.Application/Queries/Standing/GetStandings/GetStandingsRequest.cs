using MediatR;

namespace SoccerManager.Application.Queries.Standing.GetStandings;

/// <summary>
/// Represents a request to retrieve every standing belonging to a season and division.
/// </summary>
public sealed record GetStandingsRequest : IRequest<GetStandingsResponse>
{
    /// <summary>
    /// The identifier of the season whose standings are being requested.
    /// </summary>
    public int SeasonId { get; init; }

    /// <summary>
    /// The identifier of the division whose standings are being requested.
    /// </summary>
    public int DivisionId { get; init; }
}
