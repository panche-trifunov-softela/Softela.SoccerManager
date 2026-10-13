using SoccerManager.Domain.Enums;

namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents a manager's application to manage a team within a league, which stays pending until it is answered.
/// </summary>
public class LeagueTeamManagerApplication : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the league roster entry (the club in its league) the manager applies to manage.
    /// </summary>
    public int LeagueTeamId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the manager who applied.
    /// </summary>
    public int ManagerId { get; set; }

    /// <summary>
    /// Gets or sets where the application stands.
    /// </summary>
    public ApplicationStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the UTC moment the application was answered, or <see langword="null"/> while it is still pending.
    /// </summary>
    public DateTime? ResponseDate { get; set; }
}
