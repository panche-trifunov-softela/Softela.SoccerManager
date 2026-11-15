namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents whether the division clubs of a new league start with the real players of their teams or with empty
/// squads; guests always get their real players. The values deliberately start at 1, so that a zero fails
/// <c>IsInEnum</c> validation rather than silently meaning <see cref="Default"/>.
/// </summary>
public enum DivisionSquads : byte
{
    /// <summary>
    /// Every division club gets the real players of its team.
    /// </summary>
    Default = 1,

    /// <summary>
    /// Every division club starts with no players.
    /// </summary>
    Empty = 2,
}
