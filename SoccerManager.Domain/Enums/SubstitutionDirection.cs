namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents how a substitution moves a player compared with the club's main lineup. It is worked out when a substitution is
/// read and is never stored.
/// </summary>
public enum SubstitutionDirection : byte
{
    /// <summary>
    /// The player is in the substitution's starting eleven but not in the main one.
    /// </summary>
    ComingOn = 1,

    /// <summary>
    /// The player is in the main starting eleven but on the substitution's bench.
    /// </summary>
    GoingOff = 2,
}
