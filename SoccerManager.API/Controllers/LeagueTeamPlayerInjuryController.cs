using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.API.Security;
using SoccerManager.Application.Commands.LeagueTeamPlayerInjury.CreateLeagueTeamPlayerInjury;
using SoccerManager.Application.Commands.LeagueTeamPlayerInjury.DeleteLeagueTeamPlayerInjury;
using SoccerManager.Application.Commands.LeagueTeamPlayerInjury.UpdateLeagueTeamPlayerInjury;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.LeagueTeamPlayerInjury.GetLeagueTeamPlayerInjuries;
using SoccerManager.Application.Queries.LeagueTeamPlayerInjury.GetLeagueTeamPlayerInjuryById;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for the injuries of squad rows. Any signed-in user can read them;
/// only an operator can write them.
/// </summary>
[ApiController]
[Authorize]
[Route("api/league-team-player-injuries")]
public class LeagueTeamPlayerInjuryController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamPlayerInjuryController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public LeagueTeamPlayerInjuryController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every injury of the given squad row, the latest first.
    /// </summary>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of injuries of the squad row, empty when there are none; a 400 when the identifier is not positive.</returns>
    [HttpGet]
    public async Task<IActionResult> GetLeagueTeamPlayerInjuries([FromQuery] int leagueTeamPlayerId, CancellationToken cancellationToken)
    {
        // leagueTeamPlayerId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamPlayerInjuriesRequest { LeagueTeamPlayerId = leagueTeamPlayerId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the injury with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the injury to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching injury, or a 404 when no such injury exists.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeagueTeamPlayerInjuryById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamPlayerInjuryByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new injury on the given squad row.
    /// </summary>
    /// <param name="request">The injury to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created injury; a 400 when a value is invalid or the injury ends before it starts; a 403 when the caller is not an operator; or a 404 when the squad row does not exist.</returns>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> CreateLeagueTeamPlayerInjury([FromBody] CreateLeagueTeamPlayerInjuryRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateLeagueTeamPlayerInjuryRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetLeagueTeamPlayerInjuryById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the injury with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the injury to update.</param>
    /// <param name="request">The updated injury values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded; a 400 when a value is invalid or the injury ends before it starts; a 403 when the caller is not an operator; or a 404 when the injury or the squad row does not exist.</returns>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> UpdateLeagueTeamPlayerInjury(int id, [FromBody] UpdateLeagueTeamPlayerInjuryRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateLeagueTeamPlayerInjuryRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the injury with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the injury to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded; a 403 when the caller is not an operator, or a 404 when no such injury exists.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> DeleteLeagueTeamPlayerInjury(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteLeagueTeamPlayerInjuryRequest>(new DeleteLeagueTeamPlayerInjuryRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
