namespace SoccerManager.Domain.Enums;

/// <summary>
/// Represents the football confederation a national team belongs to, as stored in
/// <c>dbo.NationalTeams.Confederation</c>. A national team with none set has no value at all. The values deliberately
/// start at 1, like <see cref="MatchStage"/>, and <c>CK_NationalTeams_ConfederationKnown</c> accepts exactly these
/// values, so a new member needs a new script as well.
/// </summary>
public enum Confederation : byte
{
    /// <summary>
    /// The Asian Football Confederation.
    /// </summary>
    Afc = 1,

    /// <summary>
    /// The Confederation of African Football.
    /// </summary>
    Caf = 2,

    /// <summary>
    /// The Confederation of North, Central America and Caribbean Association Football.
    /// </summary>
    Concacaf = 3,

    /// <summary>
    /// The South American Football Confederation.
    /// </summary>
    Conmebol = 4,

    /// <summary>
    /// The Oceania Football Confederation.
    /// </summary>
    Ofc = 5,

    /// <summary>
    /// The Union of European Football Associations.
    /// </summary>
    Uefa = 6,
}
