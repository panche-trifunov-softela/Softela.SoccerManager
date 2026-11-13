using SoccerManager.Domain.Enums;

namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents one injury of a squad row (one player's registration at one club of a league): its name, when it started and
/// ended, and how serious it was.
/// </summary>
public class LeagueTeamPlayerInjury : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the squad row (the player's registration at the club) the injury is recorded on.
    /// </summary>
    public int LeagueTeamPlayerId { get; set; }

    /// <summary>
    /// Gets or sets the name of the injury, as shown to the user.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time the injury started.
    /// </summary>
    public DateTime InjuryStartDate { get; set; }

    /// <summary>
    /// Gets or sets the UTC date and time the injury ended. It is never before <see cref="InjuryStartDate"/>.
    /// </summary>
    public DateTime InjuryEndDate { get; set; }

    /// <summary>
    /// Gets or sets how serious the injury was.
    /// </summary>
    public InjuryScale InjuryScale { get; set; }
}
