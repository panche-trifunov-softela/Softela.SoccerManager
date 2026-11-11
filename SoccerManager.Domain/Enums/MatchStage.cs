namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents where a cup or playoff match sits in its competition, as stored in <c>dbo.Matches.Stage</c>. A league
/// match or a friendly has no stage. The values deliberately start at 1, like <see cref="CompetitionFormat"/>, and
/// <c>CK_Matches_StageKnown</c> accepts exactly these values, so a new member needs a new script as well.
/// </summary>
public enum MatchStage : byte
{
    /// <summary>
    /// A round of the domestic cup, a single match; <c>RoundNumber</c> says which round, from 1 up to the final.
    /// </summary>
    DomesticCupRound = 1,

    /// <summary>
    /// A semi-final of a division's promotion playoff, a single match.
    /// </summary>
    PromotionPlayoffSemiFinal = 2,

    /// <summary>
    /// The final of a division's promotion playoff, a single match.
    /// </summary>
    PromotionPlayoffFinal = 3,

    /// <summary>
    /// A league-phase match of a continental cup; <c>RoundNumber</c> is the matchday, from 1.
    /// </summary>
    ContinentalLeaguePhase = 4,

    /// <summary>
    /// A knockout play-off tie of a continental cup, played over two legs; <c>Leg</c> is 1 or 2.
    /// </summary>
    ContinentalKnockoutPlayOff = 5,

    /// <summary>
    /// A round-of-16 tie of a continental cup, played over two legs; <c>Leg</c> is 1 or 2.
    /// </summary>
    ContinentalRoundOf16 = 6,

    /// <summary>
    /// A quarter-final of a continental cup, played over two legs; <c>Leg</c> is 1 or 2.
    /// </summary>
    ContinentalQuarterFinal = 7,

    /// <summary>
    /// A semi-final of a continental cup, played over two legs; <c>Leg</c> is 1 or 2.
    /// </summary>
    ContinentalSemiFinal = 8,

    /// <summary>
    /// The final of a continental cup, a single match.
    /// </summary>
    ContinentalFinal = 9,
}
