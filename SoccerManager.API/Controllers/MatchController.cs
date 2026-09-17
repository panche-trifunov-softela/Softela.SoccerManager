using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.Match.CreateMatch;
using SoccerManager.Application.Commands.Match.DeleteMatch;
using SoccerManager.Application.Commands.Match.UpdateMatch;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.Match.GetMatchById;
using SoccerManager.Application.Queries.Match.GetMatches;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for matches.
/// </summary>
[ApiController]
[Authorize]
[Route("api/matches")]
public class MatchController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public MatchController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every match.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of every match.</returns>
    [HttpGet]
    public async Task<IActionResult> GetMatches(CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetMatchesRequest(), cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the match with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching match.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMatchById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetMatchByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new match.
    /// </summary>
    /// <param name="request">The match to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created match.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateMatch([FromBody] CreateMatchRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateMatchRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetMatchById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the match with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match to update.</param>
    /// <param name="request">The updated match values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMatch(int id, [FromBody] UpdateMatchRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateMatchRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the match with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMatch(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteMatchRequest>(new DeleteMatchRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
