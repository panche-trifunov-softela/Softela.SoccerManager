using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="LeagueTeamPlayerLoan"/> entities.
/// </summary>
public interface ILeagueTeamPlayerLoanRepository
{
    /// <summary>
    /// Persists a new loan and returns its generated identifier.
    /// </summary>
    /// <param name="leagueTeamPlayerLoan">The loan to create.</param>
    /// <returns>The identifier assigned to the created loan.</returns>
    Task<int> CreateAsync(LeagueTeamPlayerLoan leagueTeamPlayerLoan);

    /// <summary>
    /// Persists changes to an existing loan.
    /// </summary>
    /// <param name="leagueTeamPlayerLoan">The loan with updated values.</param>
    /// <returns>The identifier of the updated loan.</returns>
    Task<int> UpdateAsync(LeagueTeamPlayerLoan leagueTeamPlayerLoan);

    /// <summary>
    /// Removes a loan by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the loan to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a loan by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the loan to retrieve.</param>
    /// <returns>The matching loan, or <see langword="null"/> when none exists.</returns>
    Task<LeagueTeamPlayerLoan?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every loan of a squad row, the latest first.
    /// </summary>
    /// <param name="leagueTeamPlayerId">The identifier of the squad row whose loans to retrieve.</param>
    /// <returns>A list of every loan of the squad row.</returns>
    Task<List<LeagueTeamPlayerLoan>> GetByLeagueTeamPlayerIdAsync(int leagueTeamPlayerId);
}
