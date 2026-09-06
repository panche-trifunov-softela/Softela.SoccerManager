using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.Season.CreateSeason;
using SoccerManager.Application.Commands.Season.DeleteSeason;
using SoccerManager.Application.Commands.Season.UpdateSeason;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.Season.GetSeasonById;
using SoccerManager.Application.Queries.Season.GetSeasons;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for seasons.
/// </summary>
[ApiController]
[Authorize]
[Route("api/seasons")]
public class SeasonController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="SeasonController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public SeasonController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every season belonging to the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of seasons belonging to the league.</returns>
    [HttpGet]
    public async Task<IActionResult> GetSeasons([FromQuery] int leagueId, CancellationToken cancellationToken)
    {
        // leagueId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetSeasonsRequest { LeagueId = leagueId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the season with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the season to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching season.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSeasonById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetSeasonByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new season.
    /// </summary>
    /// <param name="request">The season to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created season.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateSeason([FromBody] CreateSeasonRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateSeasonRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetSeasonById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the season with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the season to update.</param>
    /// <param name="request">The updated season values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSeason(int id, [FromBody] UpdateSeasonRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateSeasonRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the season with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the season to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSeason(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteSeasonRequest>(new DeleteSeasonRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
