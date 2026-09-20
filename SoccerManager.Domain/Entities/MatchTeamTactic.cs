using SoccerManager.Domain.Enums;

namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents the tactical setup a team uses for a match, together with the players assigned to
/// set pieces and the captaincy.
/// </summary>
public class MatchTeamTactic : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the match the tactic applies to.
    /// </summary>
    public int MatchId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the team the tactic belongs to.
    /// </summary>
    public int TeamId { get; set; }

    /// <summary>
    /// Gets or sets the team's mentality.
    /// </summary>
    public Mentality Mentality { get; set; }

    /// <summary>
    /// Gets or sets the team's tempo.
    /// </summary>
    public Tempo Tempo { get; set; }

    /// <summary>
    /// Gets or sets the team's passing style.
    /// </summary>
    public Passing Passing { get; set; }

    /// <summary>
    /// Gets or sets the width the team plays with.
    /// </summary>
    public Width Width { get; set; }

    /// <summary>
    /// Gets or sets the height at which the team presses.
    /// </summary>
    public Pressing Pressing { get; set; }

    /// <summary>
    /// Gets or sets how aggressively the team tackles.
    /// </summary>
    public Tackling Tackling { get; set; }

    /// <summary>
    /// Gets or sets the side of the pitch the team directs its attacks through.
    /// </summary>
    public AttackingSide AttackingSide { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the player who takes penalties.
    /// </summary>
    public int PenaltyTakerPlayerId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the player who takes free kicks.
    /// </summary>
    public int FreeKickTakerPlayerId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the player who takes corner kicks.
    /// </summary>
    public int CornerKickTakerPlayerId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the player who captains the team.
    /// </summary>
    public int CaptainPlayerId { get; set; }
}
