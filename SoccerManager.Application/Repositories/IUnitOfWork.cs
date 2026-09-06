namespace SoccerManager.Application.Repositories;

/// <summary>
/// Groups the writes of a single use case into one database transaction.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
