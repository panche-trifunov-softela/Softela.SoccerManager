using MediatR;

namespace SoccerManager.Application.Queries.Player.GetPlayerById;

/// <summary>
/// Represents a request to retrieve a single player by identifier.
/// </summary>
public sealed record GetPlayerByIdRequest : IRequest<GetPlayerByIdResponse>
{
    /// <summary>
    /// The identifier of the player to retrieve.
    /// </summary>
    public int Id { get; init; }
}
