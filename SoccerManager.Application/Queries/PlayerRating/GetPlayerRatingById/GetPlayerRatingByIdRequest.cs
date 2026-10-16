using MediatR;

namespace SoccerManager.Application.Queries.PlayerRating.GetPlayerRatingById;

/// <summary>
/// Represents a request to retrieve a single player rating by identifier.
/// </summary>
public sealed record GetPlayerRatingByIdRequest : IRequest<GetPlayerRatingByIdResponse>
{
    /// <summary>
    /// The identifier of the player rating to retrieve.
    /// </summary>
    public int Id { get; init; }
}
