using MediatR;

namespace SoccerManager.Application.Commands.Season.UpdateSeason;

/// <summary>
/// Represents a request to update an existing season.
/// </summary>
public sealed record UpdateSeasonRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the season to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The new season number.
    /// </summary>
    public int SeasonNumber { get; init; }
}
