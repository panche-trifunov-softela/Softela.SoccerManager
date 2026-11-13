using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.API.Security;
using SoccerManager.Application.Commands.LeagueTeamPlayer.CreateLeagueTeamPlayer;
using SoccerManager.Application.Commands.LeagueTeamPlayer.DeleteLeagueTeamPlayer;
using SoccerManager.Application.Commands.LeagueTeamPlayer.UpdateLeagueTeamPlayer;
using SoccerManager.Application.Commands.LeagueTeamPlayer.UpdateLeagueTeamPlayerSquadNumber;
using SoccerManager.Application.Core.Command;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Creates, changes and releases league team players. They are read through GET api/league-teams/{id}/squad, which only
/// the club's current manager or an operator may call, because a registration holds the player's wage, morale and condition.
/// </summary>
[ApiController]
[Authorize]
[Route("api/league-team-players")]
public class LeagueTeamPlayerController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamPlayerController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    public LeagueTeamPlayerController(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    /// <summary>
    /// Creates a new league team player.
    /// </summary>
    /// <param name="request">The league team player to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created league team player.</returns>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> CreateLeagueTeamPlayer([FromBody] CreateLeagueTeamPlayerRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateLeagueTeamPlayerRequest>(request, cancellationToken);

        // No Location header: no route reads a single league team player.
        return StatusCode(StatusCodes.Status201Created, new { id });
    }

    /// <summary>
    /// Updates the league team player with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team player to update.</param>
    /// <param name="request">The updated league team player values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> UpdateLeagueTeamPlayer(int id, [FromBody] UpdateLeagueTeamPlayerRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateLeagueTeamPlayerRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Changes the squad number of the league team player with the given identifier. Allowed for an
    /// operator or the club's current manager.
    /// </summary>
    /// <param name="id">The identifier of the league team player whose squad number to change.</param>
    /// <param name="request">The new squad number.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}/squad-number")]
    public async Task<IActionResult> UpdateLeagueTeamPlayerSquadNumber(int id, [FromBody] UpdateLeagueTeamPlayerSquadNumberRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateLeagueTeamPlayerSquadNumberRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Releases the league team player with the given identifier, which also removes the player from the
    /// club's lineups for matches that have not started and removes the player's transfers, loans, injuries and
    /// swap rows. Allowed for an operator or the club's current manager.
    /// </summary>
    /// <param name="id">The identifier of the league team player to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLeagueTeamPlayer(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteLeagueTeamPlayerRequest>(new DeleteLeagueTeamPlayerRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
