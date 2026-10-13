using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Models;

/// <summary>
/// One row returned by the <c>dbo.GetAvailableLeagueTeams</c> stored procedure: a club on a league's
/// roster, denormalised with the team details the procedure joins in and whether it has a current manager. Kept
/// separate from <see cref="Dtos.AvailableLeagueTeamDto"/> because Dapper populates a result by setting
/// properties, and the Dto is an init-only record that cannot be a Dapper target.
/// </summary>
public class GetAvailableLeagueTeamsResult
{
    /// <summary>
    /// The identifier of the club's entry on the league's roster.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The name of the team.
    /// </summary>
    public string TeamName { get; set; } = string.Empty;

    /// <summary>
    /// The URL of the team's logo image, or <see langword="null"/> when it has none.
    /// </summary>
    public string? TeamLogoUrl { get; set; }

    /// <summary>
    /// The financial state of the team.
    /// </summary>
    public FinancialState TeamFinancialState { get; set; }

    /// <summary>
    /// Whether the club has a current manager in the league.
    /// </summary>
    public bool IsManaged { get; set; }
}
