using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.LeagueTeamPlayer.CreateLeagueTeamPlayer;
using SoccerManager.Application.Commands.LeagueTeamPlayer.DeleteLeagueTeamPlayer;
using SoccerManager.Application.Commands.LeagueTeamPlayer.UpdateLeagueTeamPlayer;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.LeagueTeamPlayer.GetLeagueTeamPlayerById;
using SoccerManager.Application.Queries.LeagueTeamPlayer.GetLeagueTeamPlayers;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for league team players.
/// </summary>
[ApiController]
[Authorize]
[Route("api/league-team-players")]
public class LeagueTeamPlayerController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamPlayerController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public LeagueTeamPlayerController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every player registration belonging to the given league and team.
    /// </summary>
    /// <param name="leagueId">The identifier of the league to filter by.</param>
    /// <param name="teamId">The identifier of the team to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of player registrations belonging to the league and team.</returns>
    [HttpGet]
    public async Task<IActionResult> GetLeagueTeamPlayers([FromQuery] int leagueId, [FromQuery] int teamId, CancellationToken cancellationToken)
    {
        // leagueId and teamId are both required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamPlayersRequest { LeagueId = leagueId, TeamId = teamId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the league team player with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team player to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching league team player.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeagueTeamPlayerById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamPlayerByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new league team player.
    /// </summary>
    /// <param name="request">The league team player to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created league team player.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateLeagueTeamPlayer([FromBody] CreateLeagueTeamPlayerRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateLeagueTeamPlayerRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetLeagueTeamPlayerById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the league team player with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team player to update.</param>
    /// <param name="request">The updated league team player values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLeagueTeamPlayer(int id, [FromBody] UpdateLeagueTeamPlayerRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateLeagueTeamPlayerRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the league team player with the given identifier.
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
