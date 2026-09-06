using MediatR;

namespace SoccerManager.Application.Core.Command;

/// <summary>
/// Dispatches a command to its registered handler.
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    /// Sends a command to its handler and returns the resulting response.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response returned by the handler.</typeparam>
    /// <typeparam name="T">The type of the command being sent.</typeparam>
    /// <param name="command">The command to dispatch.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The response produced by the command's handler.</returns>
    Task<TResponse> SendAsync<TResponse, T>(T command, CancellationToken cancellationToken) where T : IRequest<TResponse>;
}
