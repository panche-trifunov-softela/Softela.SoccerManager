using MediatR;

namespace SoccerManager.Application.Queries.Season.GetSeasons;

/// <summary>
/// Represents a request to retrieve every season belonging to a league.
/// </summary>
public sealed record GetSeasonsRequest : IRequest<GetSeasonsResponse>
{
    /// <summary>
    /// The identifier of the league whose seasons are being requested.
    /// </summary>
    public int LeagueId { get; init; }
}
