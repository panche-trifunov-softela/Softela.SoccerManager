using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.Formation.CreateFormation;
using SoccerManager.Application.Commands.Formation.DeleteFormation;
using SoccerManager.Application.Commands.Formation.UpdateFormation;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.Formation.GetFormationById;
using SoccerManager.Application.Queries.Formation.GetFormations;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for formations.
/// </summary>
[ApiController]
[Authorize]
[Route("api/formations")]
public class FormationController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="FormationController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public FormationController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every formation.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of formations.</returns>
    [HttpGet]
    public async Task<IActionResult> GetFormations(CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetFormationsRequest(), cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the formation with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching formation.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFormationById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetFormationByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new formation.
    /// </summary>
    /// <param name="request">The formation to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created formation.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateFormation([FromBody] CreateFormationRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateFormationRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetFormationById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the formation with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation to update.</param>
    /// <param name="request">The updated formation values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFormation(int id, [FromBody] UpdateFormationRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateFormationRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the formation with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the formation to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFormation(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteFormationRequest>(new DeleteFormationRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
