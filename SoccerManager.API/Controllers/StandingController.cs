using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.Standing.CreateStanding;
using SoccerManager.Application.Commands.Standing.DeleteStanding;
using SoccerManager.Application.Commands.Standing.UpdateStanding;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.Standing.GetStandingById;
using SoccerManager.Application.Queries.Standing.GetStandings;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for standings.
/// </summary>
[ApiController]
[Authorize]
[Route("api/standings")]
public class StandingController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="StandingController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public StandingController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every standing belonging to the given competition, season and division.
    /// </summary>
    /// <param name="competitionId">The identifier of the competition to filter by.</param>
    /// <param name="seasonId">The identifier of the season to filter by.</param>
    /// <param name="divisionId">The identifier of the division to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of standings belonging to the competition, season and division.</returns>
    [HttpGet]
    public async Task<IActionResult> GetStandings([FromQuery] int competitionId, [FromQuery] int seasonId, [FromQuery] int divisionId, CancellationToken cancellationToken)
    {
        // competitionId, seasonId and divisionId are all required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetStandingsRequest { CompetitionId = competitionId, SeasonId = seasonId, DivisionId = divisionId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the standing with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the standing to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching standing.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStandingById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetStandingByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new standing.
    /// </summary>
    /// <param name="request">The standing to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created standing.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateStanding([FromBody] CreateStandingRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateStandingRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetStandingById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the standing with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the standing to update.</param>
    /// <param name="request">The updated standing values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStanding(int id, [FromBody] UpdateStandingRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateStandingRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the standing with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the standing to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStanding(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteStandingRequest>(new DeleteStandingRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
