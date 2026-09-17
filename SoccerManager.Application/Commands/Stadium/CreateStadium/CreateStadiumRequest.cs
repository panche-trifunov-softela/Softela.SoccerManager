using MediatR;

namespace SoccerManager.Application.Commands.Stadium.CreateStadium;

/// <summary>
/// Represents a request to create a new stadium.
/// </summary>
public sealed record CreateStadiumRequest : IRequest<int>
{
    /// <summary>
    /// The name of the stadium to create.
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
