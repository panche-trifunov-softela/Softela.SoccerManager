using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents an injury of a squad row for read-oriented consumers.
/// </summary>
public sealed record LeagueTeamPlayerInjuryDto
{
    /// <summary>
    /// The identifier of the injury.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// The identifier of the squad row (the player's registration at the club) the injury is recorded on.
    /// </summary>
    public int LeagueTeamPlayerId { get; init; }

    /// <summary>
    /// The name of the injury, as shown to the user.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The UTC date and time the injury started.
    /// </summary>
    public DateTime InjuryStartDate { get; init; }

    /// <summary>
    /// The UTC date and time the injury ended.
    /// </summary>
    public DateTime InjuryEndDate { get; init; }

    /// <summary>
    /// How serious the injury was.
    /// </summary>
    public InjuryScale InjuryScale { get; init; }

    /// <summary>
    /// The UTC date and time the injury was created.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// The UTC date and time the injury was last modified.
    /// </summary>
    public DateTime ModifiedAt { get; init; }
}
