using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Dtos;

/// <summary>
/// Represents one injury recorded on a squad row of a player at a club of a league: what the injury was, when it started
/// and ended, and how serious it was.
/// </summary>
public sealed record PlayerInjuryDto
{
    /// <summary>
    /// The identifier of the injury.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// What the injury was, as shown to the user.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The UTC date and time the injury started.
    /// </summary>
    public DateTime InjuryStartDate { get; init; }

    /// <summary>
    /// The UTC date and time the injury ended, which is never before it started.
    /// </summary>
    public DateTime InjuryEndDate { get; init; }

    /// <summary>
    /// How serious the injury was.
    /// </summary>
    public InjuryScale InjuryScale { get; init; }
}
