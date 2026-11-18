namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents the World Cup stage a national team match belongs to, as stored in
/// <c>dbo.NationalTeamMatches.Stage</c>. The values deliberately start at 1, like <see cref="MatchStage"/>, and
/// <c>CK_NationalTeamMatches_StageKnown</c> accepts exactly these values, so a new member needs a new script as well.
/// </summary>
public enum NationalTeamMatchStage : byte
{
    /// <summary>
    /// A match in a World Cup qualifying group.
    /// </summary>
    QualifierGroup = 1,

    /// <summary>
    /// A match in a group of the World Cup finals.
    /// </summary>
    FinalsGroup = 2,

    /// <summary>
    /// A match in the round of 16, the first knockout round of the finals.
    /// </summary>
    RoundOf16 = 3,

    /// <summary>
    /// A quarter-final of the finals.
    /// </summary>
    QuarterFinal = 4,

    /// <summary>
    /// A semi-final of the finals.
    /// </summary>
    SemiFinal = 5,

    /// <summary>
    /// The match for third place, between the two beaten semi-finalists.
    /// </summary>
    ThirdPlace = 6,

    /// <summary>
    /// The World Cup final.
    /// </summary>
    Final = 7,
}
