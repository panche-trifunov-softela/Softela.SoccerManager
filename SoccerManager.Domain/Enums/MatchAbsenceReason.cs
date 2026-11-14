namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents why a player missed a match. The values deliberately start at 1, so that a zero fails
/// <c>IsInEnum</c> validation rather than silently meaning <see cref="Injured"/>.
/// <c>CK_MatchPlayerAbsences_ReasonKnown</c> on <c>dbo.MatchPlayerAbsences.Reason</c> accepts exactly
/// these values, so a new member needs a new script as well.
/// </summary>
public enum MatchAbsenceReason : byte
{
    /// <summary>
    /// The player missed the match because of an injury.
    /// </summary>
    Injured = 1,

    /// <summary>
    /// The player missed the match because of a suspension.
    /// </summary>
    Suspended = 2,
}
