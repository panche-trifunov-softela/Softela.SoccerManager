using MediatR;

namespace SoccerManager.Application.Core.Query;

/// <summary>
/// Dispatches a query to its registered handler.
/// </summary>
public interface IQueryDispatcher
{
    /// <summary>
    /// Sends a query to its handler and returns the resulting response.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
    /// <param name="query">The query to dispatch.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The response produced by the query's handler.</returns>
    Task<TResponse> QueryAsync<TResponse>(IRequest<TResponse> query, CancellationToken cancellationToken);
}
