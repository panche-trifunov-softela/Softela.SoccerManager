using SoccerManager.Application.Core.Exceptions;

namespace SoccerManager.Application.Services;

/// <summary>
/// Decides whether the current user may manage a league: an operator may manage every league, and anyone else
/// only a league they own.
/// </summary>
public interface ILeagueAccess
{
    /// <summary>
    /// Ensures the current user may manage the given league: they are an operator, or they own it.
    /// </summary>
    /// <param name="leagueId">The identifier of the league.</param>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the league's owner; a league with no owner can only be managed by an operator.</exception>
    Task EnsureCanManageLeagueAsync(int leagueId);
}
