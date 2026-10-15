using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.ManagerPlayerRatingVote.CreateManagerPlayerRatingVote;
using SoccerManager.Application.Commands.ManagerPlayerRatingVote.DeleteManagerPlayerRatingVote;
using SoccerManager.Application.Commands.ManagerPlayerRatingVote.UpdateManagerPlayerRatingVote;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.ManagerPlayerRatingVote.GetManagerPlayerRatingVoteById;
using SoccerManager.Application.Queries.ManagerPlayerRatingVote.GetManagerPlayerRatingVotes;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes the managers' votes on player ratings.
/// </summary>
[ApiController]
[Authorize]
[Route("api/manager-player-rating-votes")]
public class ManagerPlayerRatingVoteController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="ManagerPlayerRatingVoteController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public ManagerPlayerRatingVoteController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every vote on the given player, newest first.
    /// </summary>
    /// <param name="playerId">The identifier of the player to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of votes on the player.</returns>
    [HttpGet]
    public async Task<IActionResult> GetManagerPlayerRatingVotes([FromQuery] int playerId, CancellationToken cancellationToken)
    {
        // playerId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetManagerPlayerRatingVotesRequest { PlayerId = playerId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the vote with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the vote to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching vote.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetManagerPlayerRatingVoteById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetManagerPlayerRatingVoteByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Casts a vote, as the calling manager, on a player's current rating.
    /// </summary>
    /// <param name="request">The vote to cast.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created vote.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateManagerPlayerRatingVote([FromBody] CreateManagerPlayerRatingVoteRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateManagerPlayerRatingVoteRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetManagerPlayerRatingVoteById), new { id }, new { id });
    }

    /// <summary>
    /// Changes the direction of one of the calling manager's votes.
    /// </summary>
    /// <param name="id">The identifier of the vote to update.</param>
    /// <param name="request">The new direction of the vote.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateManagerPlayerRatingVote(int id, [FromBody] UpdateManagerPlayerRatingVoteRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateManagerPlayerRatingVoteRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Withdraws one of the calling manager's votes.
    /// </summary>
    /// <param name="id">The identifier of the vote to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteManagerPlayerRatingVote(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteManagerPlayerRatingVoteRequest>(new DeleteManagerPlayerRatingVoteRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
