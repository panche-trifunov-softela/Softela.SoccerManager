using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.Division.CreateDivision;
using SoccerManager.Application.Commands.Division.DeleteDivision;
using SoccerManager.Application.Commands.Division.UpdateDivision;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.Division.GetDivisionById;
using SoccerManager.Application.Queries.Division.GetDivisions;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for divisions.
/// </summary>
[ApiController]
[Authorize]
[Route("api/divisions")]
public class DivisionController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="DivisionController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public DivisionController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every division belonging to the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of divisions belonging to the league.</returns>
    [HttpGet]
    public async Task<IActionResult> GetDivisions([FromQuery] int leagueId, CancellationToken cancellationToken)
    {
        // leagueId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetDivisionsRequest { LeagueId = leagueId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the division with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the division to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching division.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDivisionById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetDivisionByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new division.
    /// </summary>
    /// <param name="request">The division to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created division.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateDivision([FromBody] CreateDivisionRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateDivisionRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetDivisionById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the division with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the division to update.</param>
    /// <param name="request">The updated division values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDivision(int id, [FromBody] UpdateDivisionRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateDivisionRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the division with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the division to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDivision(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteDivisionRequest>(new DeleteDivisionRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
