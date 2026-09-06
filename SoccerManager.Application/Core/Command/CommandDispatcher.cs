using MediatR;

namespace SoccerManager.Application.Core.Command;

/// <summary>
/// Dispatches commands to their registered handlers through MediatR.
/// </summary>
public class CommandDispatcher : ICommandDispatcher
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandDispatcher"/> class.
    /// </summary>
    /// <param name="mediator">The mediator used to send commands to their handlers.</param>
    public CommandDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Sends a command to its handler and returns the resulting response.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
    /// <typeparam name="T">The type of the command being sent.</typeparam>
    /// <param name="command">The command to dispatch.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The response produced by the command's handler.</returns>
    public async Task<TResponse> SendAsync<TResponse, T>(T command, CancellationToken cancellationToken) where T : IRequest<TResponse>
    {
        return await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
    }
}
