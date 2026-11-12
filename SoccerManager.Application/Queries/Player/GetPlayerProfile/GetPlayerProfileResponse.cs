using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.Player.GetPlayerProfile;

/// <summary>
/// Represents the result of a <see cref="GetPlayerProfileRequest"/> query.
/// </summary>
public sealed record GetPlayerProfileResponse
{
    /// <summary>
    /// The requested player's profile.
    /// </summary>
    public required PlayerProfileDto Data { get; init; }
}
