using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.API.Security;
using SoccerManager.Application.Commands.LeagueTeamPlayerLoan.CreateLeagueTeamPlayerLoan;
using SoccerManager.Application.Commands.LeagueTeamPlayerLoan.DeleteLeagueTeamPlayerLoan;
using SoccerManager.Application.Commands.LeagueTeamPlayerLoan.UpdateLeagueTeamPlayerLoan;
using SoccerManager.Application.Core.Command;
using SoccerManager.Application.Core.Query;
using SoccerManager.Application.Queries.LeagueTeamPlayerLoan.GetLeagueTeamPlayerLoanById;
using SoccerManager.Application.Queries.LeagueTeamPlayerLoan.GetLeagueTeamPlayerLoans;

namespace SoccerManager.API.Controllers;

/// <summary>
/// Exposes CRUD operations for loans of a squad row to another club. Any signed-in user can read; only operators can write.
/// </summary>
[ApiController]
[Authorize]
[Route("api/league-team-player-loans")]
public class LeagueTeamPlayerLoanController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueTeamPlayerLoanController"/> class.
    /// </summary>
    /// <param name="commandDispatcher">The dispatcher used to send commands to their handlers.</param>
    /// <param name="queryDispatcher">The dispatcher used to send queries to their handlers.</param>
    public LeagueTeamPlayerLoanController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    /// <summary>
    /// Returns every loan of the given squad row, the latest first.
    /// </summary>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row to filter by.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The list of loans of the squad row. A 400 is returned when the identifier is missing or not positive.</returns>
    [HttpGet]
    public async Task<IActionResult> GetLeagueTeamPlayerLoans([FromQuery] int leagueTeamPlayerId, CancellationToken cancellationToken)
    {
        // leagueTeamPlayerId is required; a missing or zero value is rejected as a 400 by the Application layer's validation pipeline.
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamPlayerLoansRequest { LeagueTeamPlayerId = leagueTeamPlayerId }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Returns the loan with the given identifier.
    /// </summary>
    /// <param name="id">The identifier of the loan to retrieve.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The matching loan. A 404 is returned when no loan has the identifier.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetLeagueTeamPlayerLoanById(int id, CancellationToken cancellationToken)
    {
        var result = await _queryDispatcher.QueryAsync(new GetLeagueTeamPlayerLoanByIdRequest { Id = id }, cancellationToken);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a new loan. Operators only. A 400 is returned when a value is out of range, the loan club is the lending
    /// club, or the loan club or the season is not in the player's league; a 403 when the caller is not an operator; a 404
    /// when the squad row, the loan club or the season does not exist.
    /// </summary>
    /// <param name="request">The loan to create.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>The identifier of the created loan.</returns>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> CreateLeagueTeamPlayerLoan([FromBody] CreateLeagueTeamPlayerLoanRequest request, CancellationToken cancellationToken)
    {
        var id = await _commandDispatcher.SendAsync<int, CreateLeagueTeamPlayerLoanRequest>(request, cancellationToken);

        return CreatedAtAction(nameof(GetLeagueTeamPlayerLoanById), new { id }, new { id });
    }

    /// <summary>
    /// Updates the loan with the given identifier. Operators only. A 400 is returned when a value is out of range, or when
    /// the squad row, the loan club or the season changes and the new loan club is the lending club or the loan club or the
    /// season is not in the player's league; a 403 when the caller is not an operator; a 404 when the loan, or a changed
    /// squad row, loan club or season, does not exist.
    /// </summary>
    /// <param name="id">The identifier of the loan to update.</param>
    /// <param name="request">The updated loan values.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the update succeeded.</returns>
    [HttpPut("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> UpdateLeagueTeamPlayerLoan(int id, [FromBody] UpdateLeagueTeamPlayerLoanRequest request, CancellationToken cancellationToken)
    {
        var command = request with { Id = id };
        var result = await _commandDispatcher.SendAsync<bool, UpdateLeagueTeamPlayerLoanRequest>(command, cancellationToken);

        return Ok(new { success = result });
    }

    /// <summary>
    /// Deletes the loan with the given identifier. Operators only. A 403 is returned when the caller is not an operator and
    /// a 404 when no loan has the identifier.
    /// </summary>
    /// <param name="id">The identifier of the loan to delete.</param>
    /// <param name="cancellationToken">The token used to cancel the operation.</param>
    /// <returns>Whether the deletion succeeded.</returns>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.Operator)]
    public async Task<IActionResult> DeleteLeagueTeamPlayerLoan(int id, CancellationToken cancellationToken)
    {
        var result = await _commandDispatcher.SendAsync<bool, DeleteLeagueTeamPlayerLoanRequest>(new DeleteLeagueTeamPlayerLoanRequest { Id = id }, cancellationToken);

        return Ok(new { success = result });
    }
}
