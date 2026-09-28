using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.NationalTeam.CreateNationalTeam;
using SoccerManager.Application.Commands.NationalTeam.DeleteNationalTeam;
using SoccerManager.Application.Commands.NationalTeam.UpdateNationalTeam;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.NationalTeam.GetNationalTeamById;
using SoccerManager.Application.Queries.NationalTeam.GetNationalTeams;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for national teams.
/// </summary>
[ApiController]
[Authorize]
[Route("api/national-teams")]
public class NationalTeamController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="NationalTeamController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public NationalTeamController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every national team.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of national teams.</returns>
    [HttpGet]
    public async Task<IActionResult> GetNationalTeams(CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetNationalTeamsRequest(), cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the national team with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the national team to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching national team.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNationalTeamById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetNationalTeamByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new national team.
    /// </summary>
    /// <param name="request">The national team to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created national team.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateNationalTeam([FromBody] CreateNationalTeamRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateNationalTeamRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetNationalTeamById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the national team with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the national team to update.</param>
    /// <param name="request">The updated national team values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNationalTeam(int id, [FromBody] UpdateNationalTeamRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateNationalTeamRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the national team with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the national team to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNationalTeam(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteNationalTeamRequest>(new DeleteNationalTeamRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
