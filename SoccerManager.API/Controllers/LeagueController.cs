using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.League.CreateLeague;
using SoccerManager.Application.Commands.League.DeleteLeague;
using SoccerManager.Application.Commands.League.UpdateLeague;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.League.GetLeagueById;
using SoccerManager.Application.Queries.League.GetLeagues;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for leagues.
/// </summary>
[ApiController]
[Authorize]
[Route("api/leagues")]
public class LeagueController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public LeagueController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns the newest leagues in which the caller does not currently manage a club.
    /// </summary>
    /// <param name="searchTerm">Text a league name must contain; omitted or blank means no name filter.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The available leagues, newest first.</returns>
    [HttpGet]
    public async Task<IActionResult> GetLeagues([FromQuery] string? searchTerm, CancellationToken cancellationToken)
    {
        // The caller is identified from the bearer token rather than any route or query value.
        var result = await _queryDispatcher.QueryAsync(new GetLeaguesRequest { SearchTerm = searchTerm }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the league with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the league to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching league.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeagueById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetLeagueByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new league.
    /// </summary>
    /// <param name="request">The league to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created league.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateLeague([FromBody] CreateLeagueRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateLeagueRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetLeagueById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the league with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the league to update.</param>
    /// <param name="request">The updated league values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLeague(int id, [FromBody] UpdateLeagueRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateLeagueRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the league with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the league to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLeague(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteLeagueRequest>(new DeleteLeagueRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
