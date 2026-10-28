using FluentValidation;
using SoccerManager.Application.Core.Exceptions;

namespace SoccerManager.Application.Services;

/// <summary>
/// Decides whether the current caller may change a club's squad, tactics and lineups, so every handler that
/// guards a club applies one definition of who that is.
/// </summary>
public interface IClubAccess
{
    /// <summary>
    /// Ensures the caller is an operator or the current manager of the given club.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league roster entry (the club in its league) being changed.</param>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    Task EnsureCanManageClubAsync(int leagueTeamId);

    /// <summary>
    /// Ensures the caller may change the given club's tactics or lineup for the given match: an operator
    /// always may, the club's current manager only while the club plays in a match that has not started.
    /// </summary>
    /// <param name="matchId">The identifier of the match being changed.</param>
    /// <param name="leagueTeamId">The identifier of the league roster entry (the club in its league) being changed.</param>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match or the match has already started.</exception>
    Task EnsureCanManageMatchAsync(int matchId, int leagueTeamId);
}
