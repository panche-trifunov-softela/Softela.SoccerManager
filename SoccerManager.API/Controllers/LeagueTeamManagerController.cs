using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.API.Security;
using SoccerManager.Application.Commands.LeagueTeamManager.CreateLeagueTeamManager;
using SoccerManager.Application.Commands.LeagueTeamManager.DeleteLeagueTeamManager;
using SoccerManager.Application.Commands.LeagueTeamManager.UpdateLeagueTeamManager;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.LeagueTeamManager.GetLeagueTeamManagerById;
using SoccerManager.Application.Queries.LeagueTeamManager.GetLeagueTeamManagers;
using SoccerManager.Application.Queries.LeagueTeamManager.GetMyLeagueTeamManagers;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for league team manager appointments.
/// </summary>
[ApiController]
[Authorize]
[Route("api/league-team-managers")]
public class LeagueTeamManagerController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamManagerController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public LeagueTeamManagerController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every manager appointment belonging to the given league roster entry.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league roster entry (the club in its league) to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of manager appointments belonging to the league roster entry.</returns>
    [HttpGet]
    public async Task<IActionResult> GetLeagueTeamManagers([FromQuery] int leagueTeamId, CancellationToken cancellationToken)
    {
        // leagueTeamId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamManagersRequest { LeagueTeamId = leagueTeamId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the league team manager appointments belonging to the authenticated caller.
    /// </summary>
    /// <param name="currentOnly">Whether to narrow the results to appointments that are still current.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of league team manager appointments belonging to the authenticated caller.</returns>
    [HttpGet("mine")]
    public async Task<IActionResult> GetMyLeagueTeamManagers([FromQuery] bool currentOnly, CancellationToken cancellationToken)
    {
        // The caller is identified from the bearer token rather than any route or query value.
        // A caller with no manager profile receives an empty list rather than a 404.
        var result = await _queryDispatcher.QueryAsync(new GetMyLeagueTeamManagersRequest { CurrentOnly = currentOnly }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the league team manager appointment with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team manager appointment to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching league team manager appointment.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeagueTeamManagerById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamManagerByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new league team manager appointment.
    /// </summary>
    /// <param name="request">The league team manager appointment to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created league team manager appointment.</returns>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> CreateLeagueTeamManager([FromBody] CreateLeagueTeamManagerRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateLeagueTeamManagerRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetLeagueTeamManagerById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the league team manager appointment with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team manager appointment to update.</param>
    /// <param name="request">The updated league team manager values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> UpdateLeagueTeamManager(int id, [FromBody] UpdateLeagueTeamManagerRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateLeagueTeamManagerRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the league team manager appointment with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team manager appointment to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> DeleteLeagueTeamManager(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteLeagueTeamManagerRequest>(new DeleteLeagueTeamManagerRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
