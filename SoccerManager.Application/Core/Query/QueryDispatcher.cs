using MediatR;

namespace SoccerManager.Application.Core.Query;

/// <summary>
/// Dispatches queries to their registered handlers through MediatR.
/// </summary>
public class QueryDispatcher : IQueryDispatcher
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryDispatcher"/> class.
    /// </summary>
    /// <param name="mediator">The mediator used to send queries to their handlers.</param>
    public QueryDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Sends a query to its handler and returns the resulting response.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
    /// <param name="query">The query to dispatch.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The response produced by the query's handler.</returns>
    public async Task<TResponse> QueryAsync<TResponse>(IRequest<TResponse> query, CancellationToken cancellationToken)
    {
        return await _mediator.Send(query, cancellationToken).ConfigureAwait(false);
    }
}
