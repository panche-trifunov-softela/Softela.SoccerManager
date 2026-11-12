using MediatR;

namespace SoccerManager.Application.Queries.Player.GetPlayerProfile;

/// <summary>
/// Represents a request to retrieve a player's profile from the player catalogue.
/// </summary>
public sealed record GetPlayerProfileRequest : IRequest<GetPlayerProfileResponse>
{
    /// <summary>
    /// The identifier of the player whose profile is being requested.
    /// </summary>
    public int Id { get; init; }
}
