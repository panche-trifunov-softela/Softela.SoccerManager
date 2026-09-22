using MediatR;

namespace SoccerManager.Application.Queries.Standing.GetStandings;

/// <summary>
/// Represents a request to retrieve every standing belonging to a competition, season and division.
/// </summary>
public sealed record GetStandingsRequest : IRequest<GetStandingsResponse>
{
    /// <summary>
    /// The identifier of the competition whose standings are being requested.
    /// </summary>
    public int CompetitionId { get; init; }

    /// <summary>
    /// The identifier of the season whose standings are being requested.
    /// </summary>
    public int SeasonId { get; init; }

    /// <summary>
    /// The identifier of the division whose standings are being requested.
    /// </summary>
    public int DivisionId { get; init; }
}
