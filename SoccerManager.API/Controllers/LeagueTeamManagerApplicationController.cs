using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.LeagueTeamManagerApplication.CreateLeagueTeamManagerApplication;
using SoccerManager.Application.Commands.LeagueTeamManagerApplication.DeleteLeagueTeamManagerApplication;
using SoccerManager.Application.Commands.LeagueTeamManagerApplication.UpdateLeagueTeamManagerApplication;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetLeagueTeamManagerApplicationById;
using SoccerManager.Application.Queries.LeagueTeamManagerApplication.GetLeagueTeamManagerApplications;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes operations for league team manager applications: applying to manage a team, answering an application, and reading or removing one.
/// </summary>
[ApiController]
[Authorize]
[Route("api/league-team-manager-applications")]
public class LeagueTeamManagerApplicationController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamManagerApplicationController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public LeagueTeamManagerApplicationController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every application belonging to the given league roster entry, newest first.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league roster entry (the club in its league) to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of applications belonging to the league roster entry.</returns>
    [HttpGet]
    public async Task<IActionResult> GetLeagueTeamManagerApplications([FromQuery] int leagueTeamId, CancellationToken cancellationToken)
    {
        // leagueTeamId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamManagerApplicationsRequest { LeagueTeamId = leagueTeamId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the league team manager application with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the application to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching application.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeagueTeamManagerApplicationById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamManagerApplicationByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Applies on behalf of the authenticated caller to manage a team in a league.
    /// </summary>
    /// <param name="request">The league roster entry (the club in its league) the caller applies to manage.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created application.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateLeagueTeamManagerApplication([FromBody] CreateLeagueTeamManagerApplicationRequest request, CancellationToken cancellationToken)
    {
        // The applying manager is identified from the bearer token, never from the body.
        var id = await _commandDispatcher.SendAsync<int, CreateLeagueTeamManagerApplicationRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetLeagueTeamManagerApplicationById), new { id }, new { id });
    }

    /// <summary>
    /// Answers the league team manager application with the given identifier by accepting or rejecting it.
    /// </summary>
    /// <param name="id">The identifier of the application to answer.</param>
    /// <param name="request">The answer to give.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the answer was recorded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLeagueTeamManagerApplication(int id, [FromBody] UpdateLeagueTeamManagerApplicationRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateLeagueTeamManagerApplicationRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the league team manager application with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the application to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLeagueTeamManagerApplication(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteLeagueTeamManagerApplicationRequest>(new DeleteLeagueTeamManagerApplicationRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
