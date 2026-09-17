using MediatR;

namespace SoccerManager.Application.Commands.Stadium.UpdateStadium;

/// <summary>
/// Represents a request to update an existing stadium.
/// </summary>
public sealed record UpdateStadiumRequest : IRequest<bool>
{
    /// <summary>
    /// The identifier of the stadium to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The new name of the stadium.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The URL of the stadium's image, if one has been set.
    /// </summary>
    public string? ImageUrl { get; init; }

    /// <summary>
    /// The stadium's capacity, as a number of spectators.
    /// </summary>
    public int Size { get; init; }
}
