using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.Team.CreateTeam;
using SoccerManager.Application.Commands.Team.DeleteTeam;
using SoccerManager.Application.Commands.Team.UpdateTeam;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.Team.GetTeamById;
using SoccerManager.Application.Queries.Team.GetTeams;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for teams.
/// </summary>
[ApiController]
[Authorize]
[Route("api/teams")]
public class TeamController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="TeamController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public TeamController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every team.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of teams.</returns>
    [HttpGet]
    public async Task<IActionResult> GetTeams(CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetTeamsRequest(), cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the team with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the team to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching team.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTeamById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetTeamByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new team.
    /// </summary>
    /// <param name="request">The team to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created team.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateTeam([FromBody] CreateTeamRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateTeamRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetTeamById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the team with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the team to update.</param>
    /// <param name="request">The updated team values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTeam(int id, [FromBody] UpdateTeamRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateTeamRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the team with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the team to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTeam(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteTeamRequest>(new DeleteTeamRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
