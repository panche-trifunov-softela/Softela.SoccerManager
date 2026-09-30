namespace SoccerManager.Importer.Transform;

/// <summary>
/// The broad position group a player's rating is computed relative to. Importer-internal: the domain has no
/// equivalent enum, since a player's rich position set is represented by <see cref="Model.ImportPlayerPosition"/>
/// rows instead.
/// </summary>
public enum PositionGroup
{
    /// <summary>Goalkeepers.</summary>
    Goalkeeper,

    /// <summary>Defenders.</summary>
    Defender,

    /// <summary>Midfielders.</summary>
    Midfield,

    /// <summary>Attackers.</summary>
    Attack,
}
