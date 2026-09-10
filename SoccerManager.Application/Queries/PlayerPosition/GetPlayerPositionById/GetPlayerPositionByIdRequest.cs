using MediatR;

namespace SoccerManager.Application.Queries.PlayerPosition.GetPlayerPositionById;

/// <summary>
/// Represents a request to retrieve a single player position rating by identifier.
/// </summary>
public sealed record GetPlayerPositionByIdRequest : IRequest<GetPlayerPositionByIdResponse>
{
    /// <summary>
    /// The identifier of the player position rating to retrieve.
    /// </summary>
    public int Id { get; init; }
}
