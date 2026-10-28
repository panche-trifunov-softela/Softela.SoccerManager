using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.API.Security;
using SoccerManager.Application.Commands.LeagueTeam.CreateLeagueTeam;
using SoccerManager.Application.Commands.LeagueTeam.DeleteLeagueTeam;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.LeagueTeam.GetAvailableLeagueTeams;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamById;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeamSquad;
using SoccerManager.Application.Queries.LeagueTeam.GetLeagueTeams;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes create, read and delete operations for the rosters of leagues.
/// </summary>
[ApiController]
[Authorize]
[Route("api/league-teams")]
public class LeagueTeamController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public LeagueTeamController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every team on the roster of the given league.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose roster to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of teams on the league's roster.</returns>
    [HttpGet]
    public async Task<IActionResult> GetLeagueTeams([FromQuery] int leagueId, CancellationToken cancellationToken)
    {
        // leagueId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamsRequest { LeagueId = leagueId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns every club on the given league's roster, by team name, with whether each one has a current manager there
    /// and whether the caller has a pending application for it. A pending application does not count as a manager.
    /// </summary>
    /// <param name="leagueId">The identifier of the league whose available clubs to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The league's clubs, empty for an unknown league.</returns>
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableLeagueTeams([FromQuery] int leagueId, CancellationToken cancellationToken)
    {
        // leagueId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetAvailableLeagueTeamsRequest { LeagueId = leagueId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the squad of the given league team, each player with their current rating and main position, ordered by line
    /// and position.
    /// </summary>
    /// <param name="id">The identifier of the league team whose squad to return.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The squad, empty for an unknown league team.</returns>
    [HttpGet("{id}/players")]
    public async Task<IActionResult> GetLeagueTeamSquad(int id, CancellationToken cancellationToken)
    {
        // A zero or negative id is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamSquadRequest { LeagueTeamId = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the league team with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching league team.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeagueTeamById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Adds a team to the roster of a league.
    /// </summary>
    /// <param name="request">The league and team to add to the roster.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created league team.</returns>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> CreateLeagueTeam([FromBody] CreateLeagueTeamRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateLeagueTeamRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetLeagueTeamById), new { id }, new { id });
    }

    /// <summary>
    /// Removes the league team with the given identifier from the roster, but only while nothing is
    /// attached to the club in that league. A club that still has a squad, manager appointments,
    /// applications, standings or matches there is refused with a 400 naming what blocks it.
    /// </summary>
    /// <param name="id">The identifier of the league team to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded; a 400 when something is still attached, or a 404 when no such league team exists.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> DeleteLeagueTeam(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteLeagueTeamRequest>(new DeleteLeagueTeamRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
