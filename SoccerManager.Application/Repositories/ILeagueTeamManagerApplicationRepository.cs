using SoccerManager.Application.Models;
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
    /// Retrieves every application belonging to the given league roster entry, newest first.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league roster entry to filter by.</param>
    /// <returns>A list of every application belonging to the league roster entry.</returns>
    Task<List<LeagueTeamManagerApplication>> GetByLeagueTeamIdAsync(int leagueTeamId);

    /// <summary>
    /// Retrieves a user's pending applications for the clubs of a league that have no current manager, newest first.
    /// </summary>
    /// <param name="userId">The Keycloak user identifier (the 'sub' claim) whose pending applications are retrieved.</param>
    /// <param name="leagueId">The identifier of the league whose clubs to look through.</param>
    /// <returns>A list of the user's pending applications in the league; empty when the user has no manager profile.</returns>
    Task<List<LeagueTeamManagerApplication>> GetPendingByUserAndLeagueAsync(Guid userId, int leagueId);

    /// <summary>
    /// Retrieves a user's pending applications that can still be accepted, in every league, newest first, enriched with league and team names.
    /// An application to a club that has a current manager, or in a league where the user already manages a club, is skipped.
    /// </summary>
    /// <param name="userId">The Keycloak user identifier (the 'sub' claim) whose pending applications are retrieved.</param>
    /// <returns>A list of the user's pending applications; empty when the user has no manager profile.</returns>
    Task<List<GetPendingLeagueTeamManagerApplicationsByUserIdResult>> GetPendingByUserIdAsync(Guid userId);

    /// <summary>
    /// Retrieves the pending applications that can still be accepted for clubs in the leagues a user owns, or in every league for an operator, newest first, enriched with league and team names and the applicant's name and photo.
    /// The user's own applications are skipped, and so is an application to a club that has a current manager, or in a league where the applicant already manages a club.
    /// </summary>
    /// <param name="userId">The Keycloak user identifier (the 'sub' claim) whose owned leagues are listed and whose own applications are skipped.</param>
    /// <param name="isOperator">Whether the user is an operator, which lists every league; it must come from the token, never from a request value.</param>
    /// <returns>A list of the matching applications; empty when the user owns no league and is not an operator.</returns>
    Task<List<GetPendingLeagueTeamManagerApplicationsByLeagueOwnerResult>> GetPendingByLeagueOwnerAsync(Guid userId, bool isOperator);

    /// <summary>
    /// Rejects every other pending application that accepting the given one makes moot: the other pending applications for the same club, and the applicant's other pending applications in the club's league.
    /// </summary>
    /// <param name="acceptedApplication">The application that was just accepted, carrying the response date and audit values to stamp on the rejected ones.</param>
    Task RejectOtherPendingAsync(LeagueTeamManagerApplication acceptedApplication);
}
