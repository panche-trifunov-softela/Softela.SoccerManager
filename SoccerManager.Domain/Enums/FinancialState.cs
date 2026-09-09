namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents a team's overall financial standing. The values deliberately start at 1, so that
/// an omitted or default zero fails <c>IsInEnum</c> validation rather than silently meaning
/// <see cref="VeryPoor"/>.
/// </summary>
public enum FinancialState : byte
{
    /// <summary>
    /// The team's finances are very poor.
    /// </summary>
    VeryPoor = 1,

    /// <summary>
    /// The team's finances are poor.
    /// </summary>
    Poor = 2,

    /// <summary>
    /// The team's finances are average.
    /// </summary>
    Average = 3,

    /// <summary>
    /// The team's finances are rich.
    /// </summary>
    Rich = 4,

    /// <summary>
    /// The team's finances are very rich.
    /// </summary>
    VeryRich = 5,
}
