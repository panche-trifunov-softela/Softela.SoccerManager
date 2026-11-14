using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.API.Security;
using SoccerManager.Application.Commands.MatchPlayerAbsence.CreateMatchPlayerAbsence;
using SoccerManager.Application.Commands.MatchPlayerAbsence.DeleteMatchPlayerAbsence;
using SoccerManager.Application.Commands.MatchPlayerAbsence.UpdateMatchPlayerAbsence;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.MatchPlayerAbsence.GetMatchPlayerAbsenceById;
using SoccerManager.Application.Queries.MatchPlayerAbsence.GetMatchPlayerAbsences;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for the players who missed a match, injured or suspended. Any signed-in user can read the
/// absences; only an operator can write them.
/// </summary>
[ApiController]
[Authorize]
[Route("api/match-player-absences")]
public class MatchPlayerAbsenceController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchPlayerAbsenceController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public MatchPlayerAbsenceController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every player absence recorded for the given match, ordered by club and then player.
    /// </summary>
    /// <param name="matchId">The identifier of the match to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of player absences recorded for the match, empty when there are none; a 400 when the identifier is not positive.</returns>
    [HttpGet]
    public async Task<IActionResult> GetMatchPlayerAbsences([FromQuery] int matchId, CancellationToken cancellationToken)
    {
        // matchId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetMatchPlayerAbsencesRequest { MatchId = matchId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the match player absence with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match player absence to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching match player absence, or a 404 when no such absence exists.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMatchPlayerAbsenceById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetMatchPlayerAbsenceByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new match player absence.
    /// </summary>
    /// <param name="request">The match player absence to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created match player absence; a 400 when a value is invalid, the club does not play the match, the player is not in the club's squad, the player already has an absence in the match, or the player played in it; a 403 when the caller is not an operator; or a 404 when the match does not exist.</returns>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> CreateMatchPlayerAbsence([FromBody] CreateMatchPlayerAbsenceRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateMatchPlayerAbsenceRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetMatchPlayerAbsenceById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the match player absence with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match player absence to update.</param>
    /// <param name="request">The updated match player absence values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded; a 400 when a value is invalid, or when the absence moves to a club that does not play the match, a player who is not in the club's squad, a player who already has an absence in the match, or a player who played in it; a 403 when the caller is not an operator; or a 404 when the absence or the match does not exist.</returns>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> UpdateMatchPlayerAbsence(int id, [FromBody] UpdateMatchPlayerAbsenceRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateMatchPlayerAbsenceRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the match player absence with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match player absence to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded; a 403 when the caller is not an operator, or a 404 when no such absence exists.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> DeleteMatchPlayerAbsence(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteMatchPlayerAbsenceRequest>(new DeleteMatchPlayerAbsenceRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
