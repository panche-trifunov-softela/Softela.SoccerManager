using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.Competition.CreateCompetition;
using SoccerManager.Application.Commands.Competition.DeleteCompetition;
using SoccerManager.Application.Commands.Competition.UpdateCompetition;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.Competition.GetCompetitionById;
using SoccerManager.Application.Queries.Competition.GetCompetitions;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for competitions.
/// </summary>
[ApiController]
[Authorize]
[Route("api/competitions")]
public class CompetitionController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompetitionController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public CompetitionController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every competition belonging to the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of competitions belonging to the league.</returns>
    [HttpGet]
    public async Task<IActionResult> GetCompetitions([FromQuery] int leagueId, CancellationToken cancellationToken)
    {
        // leagueId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetCompetitionsRequest { LeagueId = leagueId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the competition with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the competition to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching competition.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCompetitionById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetCompetitionByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new competition.
    /// </summary>
    /// <param name="request">The competition to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created competition.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateCompetition([FromBody] CreateCompetitionRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateCompetitionRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetCompetitionById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the competition with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the competition to update.</param>
    /// <param name="request">The updated competition values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCompetition(int id, [FromBody] UpdateCompetitionRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateCompetitionRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the competition with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the competition to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompetition(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteCompetitionRequest>(new DeleteCompetitionRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
