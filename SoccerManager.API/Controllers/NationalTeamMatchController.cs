using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.API.Security;
using SoccerManager.Application.Commands.NationalTeamMatch.CreateNationalTeamMatch;
using SoccerManager.Application.Commands.NationalTeamMatch.DeleteNationalTeamMatch;
using SoccerManager.Application.Commands.NationalTeamMatch.UpdateNationalTeamMatch;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.NationalTeamMatch.GetNationalTeamMatchById;
using SoccerManager.Application.Queries.NationalTeamMatch.GetNationalTeamMatches;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for the World Cup matches between national teams.
/// </summary>
[ApiController]
[Authorize]
[Route("api/national-team-matches")]
public class NationalTeamMatchController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="NationalTeamMatchController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public NationalTeamMatchController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every national team match of the given season, in kickoff order.
    /// </summary>
    /// <param name="seasonId">The identifier of the season to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of national team matches of the season, in kickoff order.</returns>
    [HttpGet]
    public async Task<IActionResult> GetNationalTeamMatches([FromQuery] int seasonId, CancellationToken cancellationToken)
    {
        // seasonId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetNationalTeamMatchesRequest { SeasonId = seasonId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the national team match with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the national team match to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching national team match.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNationalTeamMatchById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetNationalTeamMatchByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new national team match.
    /// </summary>
    /// <param name="request">The national team match to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created national team match.</returns>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> CreateNationalTeamMatch([FromBody] CreateNationalTeamMatchRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateNationalTeamMatchRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetNationalTeamMatchById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the national team match with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the national team match to update.</param>
    /// <param name="request">The updated national team match values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> UpdateNationalTeamMatch(int id, [FromBody] UpdateNationalTeamMatchRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateNationalTeamMatchRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the national team match with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the national team match to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> DeleteNationalTeamMatch(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteNationalTeamMatchRequest>(new DeleteNationalTeamMatchRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
