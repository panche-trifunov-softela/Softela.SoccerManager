using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.MatchTeamStatistic.CreateMatchTeamStatistic;
using SoccerManager.Application.Commands.MatchTeamStatistic.DeleteMatchTeamStatistic;
using SoccerManager.Application.Commands.MatchTeamStatistic.UpdateMatchTeamStatistic;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.MatchTeamStatistic.GetMatchTeamStatisticById;
using SoccerManager.Application.Queries.MatchTeamStatistic.GetMatchTeamStatistics;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for match team statistics.
/// </summary>
[ApiController]
[Authorize]
[Route("api/match-team-statistics")]
public class MatchTeamStatisticController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchTeamStatisticController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public MatchTeamStatisticController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every team statistic recorded for the given match.
    /// </summary>
    /// <param name="matchId">The identifier of the match to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of team statistics recorded for the match.</returns>
    [HttpGet]
    public async Task<IActionResult> GetMatchTeamStatistics([FromQuery] int matchId, CancellationToken cancellationToken)
    {
        // matchId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetMatchTeamStatisticsRequest { MatchId = matchId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the match team statistic with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team statistic to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching match team statistic.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMatchTeamStatisticById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetMatchTeamStatisticByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new match team statistic.
    /// </summary>
    /// <param name="request">The match team statistic to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created match team statistic.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateMatchTeamStatistic([FromBody] CreateMatchTeamStatisticRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateMatchTeamStatisticRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetMatchTeamStatisticById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the match team statistic with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team statistic to update.</param>
    /// <param name="request">The updated match team statistic values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMatchTeamStatistic(int id, [FromBody] UpdateMatchTeamStatisticRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateMatchTeamStatisticRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the match team statistic with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team statistic to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMatchTeamStatistic(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteMatchTeamStatisticRequest>(new DeleteMatchTeamStatisticRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
