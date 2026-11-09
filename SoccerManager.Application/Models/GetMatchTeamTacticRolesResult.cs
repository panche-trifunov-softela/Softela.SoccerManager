namespace SoccerManager.Application.Models;

/// <summary>
/// One row returned by the <c>dbo.GetMatchTeamTacticRoles</c> stored procedure: the four roles of a club's main tactic for a
/// match. Dapper populates a result by setting properties, so the row is its own class rather than a domain entity.
/// </summary>
public class GetMatchTeamTacticRolesResult
{
    /// <summary>
    /// The identifier of the player who takes penalties.
    /// </summary>
    public int PenaltyTakerPlayerId { get; set; }

    /// <summary>
    /// The identifier of the player who takes free kicks.
    /// </summary>
    public int FreeKickTakerPlayerId { get; set; }

    /// <summary>
    /// The identifier of the player who takes corner kicks.
    /// </summary>
    public int CornerKickTakerPlayerId { get; set; }

    /// <summary>
    /// The identifier of the player who captains the team.
    /// </summary>
    public int CaptainPlayerId { get; set; }
}
