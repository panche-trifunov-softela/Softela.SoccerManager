using MediatR;

namespace SoccerManager.Application.Commands.Season.DeleteSeason;

/// <summary>
/// Represents a request to delete an existing season.
/// </summary>
public sealed record DeleteSeasonRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the season to delete.
    /// </summary>
    public int Id { get; init; }
}
