using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Models;

/// <summary>
/// One injury row of the second result set returned by the <c>dbo.GetLeaguePlayerInjuries</c> stored procedure: an
/// injury recorded on a squad row of the player at a club of a league. Kept separate from
/// <see cref="Dtos.PlayerInjuryDto"/> because Dapper populates a result by setting properties, and the Dto is an
/// init-only record that cannot be a Dapper target.
/// </summary>
public class GetLeaguePlayerInjuriesResult
{
    /// <summary>
    /// The identifier of the injury.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// What the injury was, as shown to the user.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The UTC date and time the injury started.
    /// </summary>
    public DateTime InjuryStartDate { get; set; }

    /// <summary>
    /// The UTC date and time the injury ended, which is never before it started.
    /// </summary>
    public DateTime InjuryEndDate { get; set; }

    /// <summary>
    /// How serious the injury was.
    /// </summary>
    public InjuryScale InjuryScale { get; set; }
}
