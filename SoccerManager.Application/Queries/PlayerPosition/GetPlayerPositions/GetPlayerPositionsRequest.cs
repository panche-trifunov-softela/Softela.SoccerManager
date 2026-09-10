using MediatR;

namespace SoccerManager.Application.Queries.PlayerPosition.GetPlayerPositions;

/// <summary>
/// Represents a request to retrieve every position rating belonging to a player.
/// </summary>
public sealed record GetPlayerPositionsRequest : IRequest<GetPlayerPositionsResponse>
{
    /// <summary>
    /// The identifier of the player whose position ratings are being requested.
    /// </summary>
    public int PlayerId { get; init; }
}
