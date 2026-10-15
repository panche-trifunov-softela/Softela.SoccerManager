using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Commands.Player.CreatePlayer;
using SoccerManager.Application.Commands.Player.DeletePlayer;
using SoccerManager.Application.Commands.Player.UpdatePlayer;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.Player.GetPlayerById;
using SoccerManager.Application.Queries.Player.GetPlayers;
using SoccerManager.Application.Queries.Player.SearchPlayers;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for players.
/// </summary>
[ApiController]
[Authorize]
[Route("api/players")]
public class PlayerController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public PlayerController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every player.
    /// </summary>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of players.</returns>
    [HttpGet]
    public async Task<IActionResult> GetPlayers(CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetPlayersRequest(), cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns one page of the players matching the given search, filters and sort order.
    /// </summary>
    /// <param name="searchTerm">Text a player name must contain, ignoring case and accents; omitted or blank means no name filter.</param>
    /// <param name="positionId">The identifier of the main position a player must have.</param>
    /// <param name="teamId">The identifier of the current real-life club a player must have.</param>
    /// <param name="nationalTeamId">The identifier of the national team a player must have.</param>
    /// <param name="minRating">The lowest rating to include, from 1 to 100.</param>
    /// <param name="maxRating">The highest rating to include, from 1 to 100 and not below <paramref name="minRating"/>.</param>
    /// <param name="sortBy">The column to order by: name, rating, age (ascending is youngest first), value or wage, in any case; rating when omitted.</param>
    /// <param name="sortDescending">Whether to order in descending order; descending when omitted.</param>
    /// <param name="page">The 1-based page to return; the first when omitted.</param>
    /// <param name="pageSize">The number of players per page, from 1 to 100; 25 when omitted.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The page of players with the total match count, and the page and page size applied.</returns>
    [HttpGet("search")]
    public async Task<IActionResult> SearchPlayers(
        [FromQuery] string? searchTerm,
        [FromQuery] int? positionId,
        [FromQuery] int? teamId,
        [FromQuery] int? nationalTeamId,
        [FromQuery] int? minRating,
        [FromQuery] int? maxRating,
        [FromQuery] string? sortBy,
        [FromQuery] bool? sortDescending,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        CancellationToken cancellationToken)
    {
        var request = new SearchPlayersRequest
        {
            SearchTerm = searchTerm,
            PositionId = positionId,
            TeamId = teamId,
            NationalTeamId = nationalTeamId,
            MinRating = minRating,
            MaxRating = maxRating,
            SortBy = sortBy,
            SortDescending = sortDescending,
            Page = page,
            PageSize = pageSize,
        };

        var result = await _queryDispatcher.QueryAsync(request, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the player with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the player to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching player.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPlayerById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetPlayerByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new player.
    /// </summary>
    /// <param name="request">The player to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created player.</returns>
    [HttpPost]
    public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreatePlayerRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetPlayerById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the player with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the player to update.</param>
    /// <param name="request">The updated player values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer(int id, [FromBody] UpdatePlayerRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdatePlayerRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the player with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the player to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeletePlayerRequest>(new DeletePlayerRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
