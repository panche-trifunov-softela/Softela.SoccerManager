using SoccerManager.Domain.Enums;

namespace SoccerManager.Domain.Entities;

/// <summary>
/// Represents one World Cup match between two national teams in a league's season, qualifiers and finals alike.
/// </summary>
public class NationalTeamMatch : BaseEntity
{
    /// <summary>
    /// Gets or sets the identifier of the season the match is played in.
    /// </summary>
    public int SeasonId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the home national team.
    /// </summary>
    public int HomeNationalTeamId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the away national team. It differs from the home national team.
    /// </summary>
    public int AwayNationalTeamId { get; set; }

    /// <summary>
    /// Gets or sets the UTC moment the match starts.
    /// </summary>
    public DateTime StartDateTime { get; set; }

    /// <summary>
    /// Gets or sets the World Cup stage the match belongs to.
    /// </summary>
    public NationalTeamMatchStage Stage { get; set; }

    /// <summary>
    /// Gets or sets the group within the stage, counted from 1, or <see langword="null"/> when the stage has no groups.
    /// </summary>
    public byte? GroupNumber { get; set; }

    /// <summary>
    /// Gets or sets the matchday within the group, counted from 1, or <see langword="null"/> when it does not apply.
    /// </summary>
    public byte? RoundNumber { get; set; }

    /// <summary>
    /// Gets or sets the leg of a two-legged tie, 1 or 2, or <see langword="null"/> when the match is a single match.
    /// </summary>
    public byte? Leg { get; set; }

    /// <summary>
    /// Gets or sets whether the match has started.
    /// </summary>
    public bool IsStarted { get; set; }

    /// <summary>
    /// Gets or sets whether the match has finished. A finished match has always started.
    /// </summary>
    public bool IsFinished { get; set; }
}
