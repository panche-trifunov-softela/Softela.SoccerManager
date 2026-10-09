using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="LeagueTeamManagerApplication"/> entities.
/// </summary>
public interface ILeagueTeamManagerApplicationRepository
{
    /// <summary>
    /// Persists a new league team manager application and returns its generated identifier.
    /// </summary>
    /// <param name="application">The league team manager application to create.</param>
    /// <returns>The identifier assigned to the created league team manager application.</returns>
    Task<int> CreateAsync(LeagueTeamManagerApplication application);

    /// <summary>
    /// Persists the answer to an existing league team manager application.
    /// </summary>
    /// <param name="application">The league team manager application with its updated status and response date.</param>
    /// <returns>The identifier of the answered league team manager application, or 0 when it was no longer pending.</returns>
    Task<int> UpdateAsync(LeagueTeamManagerApplication application);

    /// <summary>
    /// Removes a league team manager application by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team manager application to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a league team manager application by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the league team manager application to retrieve.</param>
    /// <returns>The matching league team manager application, or <see langword="null"/> when none exists.</returns>
    Task<LeagueTeamManagerApplication?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every application belonging to the given league and team, newest first.
    /// </summary>
    /// <param name="leagueId">The identifier of the league to filter by.</param>
    /// <param name="teamId">The identifier of the team to filter by.</param>
    /// <returns>A list of every application belonging to the league and team.</returns>
    Task<List<LeagueTeamManagerApplication>> GetByLeagueAndTeamAsync(int leagueId, int teamId);

    /// <summary>
    /// Rejects every other pending application that accepting the given one makes moot: the other pending applications for the same team in the league, and the applicant's other pending applications in the league.
    /// </summary>
    /// <param name="acceptedApplication">The application that was just accepted, carrying the response date and audit values to stamp on the rejected ones.</param>
    Task RejectOtherPendingAsync(LeagueTeamManagerApplication acceptedApplication);
}
