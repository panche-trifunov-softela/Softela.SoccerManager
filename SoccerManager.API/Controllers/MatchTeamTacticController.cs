using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.MatchTeamTactic.CreateMatchTeamTactic;
using SoccerManager.Application.Commands.MatchTeamTactic.DeleteMatchTeamTactic;
using SoccerManager.Application.Commands.MatchTeamTactic.UpdateMatchTeamTactic;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.MatchTeamTactic.GetMatchTeamTacticById;
using SoccerManager.Application.Queries.MatchTeamTactic.GetMatchTeamTactics;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for match team tactics.
/// </summary>
[ApiController]
[Authorize]
[Route("api/match-team-tactics")]
public class MatchTeamTacticController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchTeamTacticController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public MatchTeamTacticController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every team tactic recorded for the given match.
    /// </summary>
    /// <param name="matchId">The identifier of the match to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of team tactics recorded for the match.</returns>
    [HttpGet]
    public async Task<IActionResult> GetMatchTeamTactics([FromQuery] int matchId, CancellationToken cancellationToken)
    {
        // matchId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetMatchTeamTacticsRequest { MatchId = matchId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the match team tactic with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team tactic to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching match team tactic.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMatchTeamTacticById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetMatchTeamTacticByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new match team tactic.
    /// </summary>
    /// <param name="request">The match team tactic to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created match team tactic.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateMatchTeamTactic([FromBody] CreateMatchTeamTacticRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateMatchTeamTacticRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetMatchTeamTacticById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the match team tactic with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team tactic to update.</param>
    /// <param name="request">The updated match team tactic values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMatchTeamTactic(int id, [FromBody] UpdateMatchTeamTacticRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateMatchTeamTacticRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the match team tactic with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team tactic to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMatchTeamTactic(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteMatchTeamTacticRequest>(new DeleteMatchTeamTacticRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
