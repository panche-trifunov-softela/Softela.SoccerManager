using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.PlayerRating.CreatePlayerRating;
using SoccerManager.Application.Commands.PlayerRating.DeletePlayerRating;
using SoccerManager.Application.Commands.PlayerRating.UpdatePlayerRating;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.PlayerRating.GetPlayerRatingById;
using SoccerManager.Application.Queries.PlayerRating.GetPlayerRatings;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes players' rating history; the current rating is the one in force.
/// </summary>
[ApiController]
[Authorize]
[Route("api/player-ratings")]
public class PlayerRatingController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerRatingController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public PlayerRatingController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every rating the given player has had, newest first.
    /// </summary>
    /// <param name="playerId">The identifier of the player to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of ratings the player has had.</returns>
    [HttpGet]
    public async Task<IActionResult> GetPlayerRatings([FromQuery] int playerId, CancellationToken cancellationToken)
    {
        // playerId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetPlayerRatingsRequest { PlayerId = playerId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the player rating with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the player rating to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching player rating.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlayerRatingById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetPlayerRatingByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Gives a player a new current rating; the previous one is kept as history and its votes stay with it.
    /// </summary>
    /// <param name="request">The player and the new rating.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created player rating.</returns>
    [HttpPost]
    public async Task<IActionResult> CreatePlayerRating([FromBody] CreatePlayerRatingRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreatePlayerRatingRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetPlayerRatingById), new { id }, new { id });
    }

    /// <summary>
    /// Corrects the value of a rating.
    /// </summary>
    /// <param name="id">The identifier of the player rating to update.</param>
    /// <param name="request">The corrected rating value.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayerRating(int id, [FromBody] UpdatePlayerRatingRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdatePlayerRatingRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes a past rating and the votes cast on it; the current rating cannot be deleted.
    /// </summary>
    /// <param name="id">The identifier of the player rating to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayerRating(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeletePlayerRatingRequest>(new DeletePlayerRatingRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
