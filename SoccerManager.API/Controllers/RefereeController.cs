using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.Referee.CreateReferee;
using SoccerManager.Application.Commands.Referee.DeleteReferee;
using SoccerManager.Application.Commands.Referee.UpdateReferee;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.Referee.GetRefereeById;
using SoccerManager.Application.Queries.Referee.GetReferees;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for referees.
/// </summary>
[ApiController]
[Authorize]
[Route("api/referees")]
public class RefereeController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="RefereeController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public RefereeController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every referee.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of referees.</returns>
    [HttpGet]
    public async Task<IActionResult> GetReferees(CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetRefereesRequest(), cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the referee with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the referee to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching referee.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRefereeById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetRefereeByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new referee.
    /// </summary>
    /// <param name="request">The referee to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created referee.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateReferee([FromBody] CreateRefereeRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateRefereeRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetRefereeById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the referee with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the referee to update.</param>
    /// <param name="request">The updated referee values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReferee(int id, [FromBody] UpdateRefereeRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateRefereeRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the referee with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the referee to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReferee(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteRefereeRequest>(new DeleteRefereeRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
