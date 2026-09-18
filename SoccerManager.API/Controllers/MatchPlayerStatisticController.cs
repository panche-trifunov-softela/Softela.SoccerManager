using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.MatchPlayerStatistic.CreateMatchPlayerStatistic;
using SoccerManager.Application.Commands.MatchPlayerStatistic.DeleteMatchPlayerStatistic;
using SoccerManager.Application.Commands.MatchPlayerStatistic.UpdateMatchPlayerStatistic;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.MatchPlayerStatistic.GetMatchPlayerStatisticById;
using SoccerManager.Application.Queries.MatchPlayerStatistic.GetMatchPlayerStatistics;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for match player statistics.
/// </summary>
[ApiController]
[Authorize]
[Route("api/match-player-statistics")]
public class MatchPlayerStatisticController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchPlayerStatisticController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public MatchPlayerStatisticController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every player statistic recorded for the given match.
    /// </summary>
    /// <param name="matchId">The identifier of the match to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of player statistics recorded for the match.</returns>
    [HttpGet]
    public async Task<IActionResult> GetMatchPlayerStatistics([FromQuery] int matchId, CancellationToken cancellationToken)
    {
        // matchId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetMatchPlayerStatisticsRequest { MatchId = matchId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the match player statistic with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match player statistic to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching match player statistic.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMatchPlayerStatisticById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetMatchPlayerStatisticByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new match player statistic.
    /// </summary>
    /// <param name="request">The match player statistic to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created match player statistic.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateMatchPlayerStatistic([FromBody] CreateMatchPlayerStatisticRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateMatchPlayerStatisticRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetMatchPlayerStatisticById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the match player statistic with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match player statistic to update.</param>
    /// <param name="request">The updated match player statistic values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMatchPlayerStatistic(int id, [FromBody] UpdateMatchPlayerStatisticRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateMatchPlayerStatisticRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the match player statistic with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the match player statistic to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMatchPlayerStatistic(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteMatchPlayerStatisticRequest>(new DeleteMatchPlayerStatisticRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
