namespace SoccerManager.Importer.Database.Changes;

/// <summary>
/// Whether a planned row write creates a new row or updates an existing one.
/// </summary>
public enum ChangeKind
{
    /// <summary>The row does not exist yet and will be created.</summary>
    Create = 1,

    /// <summary>The row exists and will be updated.</summary>
    Update = 2,
}
