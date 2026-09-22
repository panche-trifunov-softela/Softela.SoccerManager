namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents the format a competition is played in. The values deliberately start at 1, so that
/// an omitted or default zero fails <c>IsInEnum</c> validation rather than silently meaning
/// <see cref="KnockoutOnly"/>.
/// </summary>
public enum CompetitionFormat : byte
{
    /// <summary>
    /// The competition is played entirely as a knockout.
    /// </summary>
    KnockoutOnly = 1,

    /// <summary>
    /// The competition is played entirely in groups.
    /// </summary>
    GroupOnly = 2,

    /// <summary>
    /// The competition is played in a group stage followed by a knockout stage.
    /// </summary>
    GroupAndKnockout = 3,
}
