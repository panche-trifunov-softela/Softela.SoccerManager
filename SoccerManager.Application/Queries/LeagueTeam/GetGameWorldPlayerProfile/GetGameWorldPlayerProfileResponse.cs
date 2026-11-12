using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.LeagueTeam.GetGameWorldPlayerProfile;

/// <summary>
/// Represents the result of a <see cref="GetGameWorldPlayerProfileRequest"/> query.
/// </summary>
public sealed record GetGameWorldPlayerProfileResponse
{
    /// <summary>
    /// The requested player's profile in the game world.
    /// </summary>
    public required SquadPlayerProfileDto Data { get; init; }
}
