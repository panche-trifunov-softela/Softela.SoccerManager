using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.Stadium.CreateStadium;
using SoccerManager.Application.Commands.Stadium.DeleteStadium;
using SoccerManager.Application.Commands.Stadium.UpdateStadium;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.Stadium.GetStadiumById;
using SoccerManager.Application.Queries.Stadium.GetStadiums;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for stadiums.
/// </summary>
[ApiController]
[Authorize]
[Route("api/stadiums")]
public class StadiumController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="StadiumController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public StadiumController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every stadium.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of every stadium.</returns>
    [HttpGet]
    public async Task<IActionResult> GetStadiums(CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetStadiumsRequest(), cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the stadium with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the stadium to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching stadium.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStadiumById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetStadiumByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new stadium.
    /// </summary>
    /// <param name="request">The stadium to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created stadium.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateStadium([FromBody] CreateStadiumRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateStadiumRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetStadiumById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the stadium with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the stadium to update.</param>
    /// <param name="request">The updated stadium values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStadium(int id, [FromBody] UpdateStadiumRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateStadiumRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the stadium with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the stadium to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStadium(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteStadiumRequest>(new DeleteStadiumRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
