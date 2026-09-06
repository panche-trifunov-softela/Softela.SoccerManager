using MediatR;

namespace SoccerManager.Application.Commands.Season.CreateSeason;

/// <summary>
/// Represents a request to create a new season.
/// </summary>
public sealed record CreateSeasonRequest : IRequest<int>
{
    /// <summary>
    /// The identifier of the league the season belongs to.
    /// </summary>
    public int LeagueId { get; init; }

    /// <summary>
    /// The number of the season to create.
    /// </summary>
    public int SeasonNumber { get; init; }
}
