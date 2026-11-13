namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents how serious an injury of a player is. The values deliberately start at 1, so that a zero fails
/// <c>IsInEnum</c> validation rather than silently meaning <see cref="Minor"/>.
/// <c>CK_LeagueTeamPlayerInjuries_InjuryScaleKnown</c> on <c>dbo.LeagueTeamPlayerInjuries.InjuryScale</c> accepts exactly
/// these values, so a new member needs a new script as well.
/// </summary>
public enum InjuryScale : byte
{
    /// <summary>
    /// The injury is minor.
    /// </summary>
    Minor = 1,

    /// <summary>
    /// The injury is moderate.
    /// </summary>
    Moderate = 2,

    /// <summary>
    /// The injury is serious.
    /// </summary>
    Serious = 3,
}
