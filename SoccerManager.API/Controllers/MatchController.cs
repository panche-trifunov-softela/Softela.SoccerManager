using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SoccerManager.API.RateLimiting;
using SoccerManager.API.Security;
using SoccerManager.Application.Commands.Match.AutoPickMatchLineup;
using SoccerManager.Application.Commands.Match.CreateMatch;
using SoccerManager.Application.Commands.Match.DeleteMatch;
using SoccerManager.Application.Commands.Match.SaveMatchInstructions;
using SoccerManager.Application.Commands.Match.SaveMatchLineup;
using SoccerManager.Application.Commands.Match.UpdateMatch;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Dtos;
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
    [EnableRateLimiting(RateLimitPolicies.FullTable)]
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
    [Authorize(Policy = AuthorizationPolicies.Operator)]
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
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> UpdateMatch(int id, [FromBody] UpdateMatchRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateMatchRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Changes a club's lineup for the match with the given identifier: its formation, one slot, or both. Allowed for an
    /// operator, or the club's current manager while the club plays the match and it has not started.
    /// </summary>
    /// <param name="id">The identifier of the match whose lineup to change.</param>
    /// <param name="request">The club, and the formation change, the slot change, or both to apply.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The club's lineup after the change.</returns>
    [HttpPut("{id}/lineup")]
    public async Task<IActionResult> SaveMatchLineup(int id, [FromBody] SaveMatchLineupRequest request, CancellationToken cancellationToken)
    {
        var command = request with { MatchId = id };
        var lineup = await _commandDispatcher.SendAsync<MatchLineupDto, SaveMatchLineupRequest>(command, cancellationToken);

        // Returns the lineup rather than { success }, because a client redraws its pitch from it.
        return Ok(lineup);
    }

    /// <summary>
    /// Replaces a club's lineup for the match with the given identifier with one picked from its squad. Allowed for an
    /// operator, or the club's current manager while the club plays the match and it has not started.
    /// </summary>
    /// <param name="id">The identifier of the match whose lineup to pick.</param>
    /// <param name="request">The club, and optionally the formation to pick for.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The club's picked lineup.</returns>
    [HttpPost("{id}/lineup/auto-pick")]
    public async Task<IActionResult> AutoPickMatchLineup(int id, [FromBody] AutoPickMatchLineupRequest request, CancellationToken cancellationToken)
    {
        var command = request with { MatchId = id };
        var lineup = await _commandDispatcher.SendAsync<MatchLineupDto, AutoPickMatchLineupRequest>(command, cancellationToken);

        // A 200 rather than a 201: the pick replaces the lineup instead of creating a resource, and the body is the new lineup.
        return Ok(lineup);
    }

    /// <summary>
    /// Changes one or more of a club's instructions for the match with the given identifier; the others keep their value.
    /// Allowed for an operator, or the club's current manager while the club plays the match and it has not started.
    /// </summary>
    /// <param name="id">The identifier of the match whose instructions to change.</param>
    /// <param name="request">The club, and the instructions to change.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The club's instructions after the change.</returns>
    [HttpPut("{id}/instructions")]
    public async Task<IActionResult> SaveMatchInstructions(int id, [FromBody] SaveMatchInstructionsRequest request, CancellationToken cancellationToken)
    {
        var command = request with { MatchId = id };
        var instructions = await _commandDispatcher.SendAsync<MatchInstructionsDto, SaveMatchInstructionsRequest>(command, cancellationToken);

        // Returns the instructions rather than { success }, because a client redraws its controls from them.
        return Ok(instructions);
    }

    /// <summary>
    /// Deletes the match with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> DeleteMatch(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteMatchRequest>(new DeleteMatchRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
