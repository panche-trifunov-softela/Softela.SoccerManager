using SoccerManager.Application.Core.User;

namespace SoccerManager.Importer.Database;

/// <summary>
/// The fixed <see cref="ICurrentUser"/> every row the importer writes is stamped with, since the importer runs
/// unattended and has no authenticated caller of its own to report.
/// </summary>
public sealed class ImportCurrentUser : ICurrentUser
{
    /// <summary>The fixed user id every row the importer writes is stamped with as CreatedBy/ModifiedBy.</summary>
    public static readonly Guid ImportUserId = new("00000000-0000-0000-0000-000000000001");

    /// <summary>Gets the fixed <see cref="ImportUserId"/>.</summary>
    public Guid UserId => ImportUserId;

    /// <summary>
    /// Gets a value indicating whether the caller is an operator. Always <see langword="true"/>: the importer is a
    /// trusted batch job run by an operator, and it never calls the handlers that check club ownership.
    /// </summary>
    public bool IsOperator => true;
}
