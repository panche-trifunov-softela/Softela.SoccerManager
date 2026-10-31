using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;

namespace SoccerManager.Application.Services;

/// <summary>
/// Decides whether the current user may manage a league: an operator may manage every league, and anyone else
/// only a league they own (dbo.Leagues.OwnerManagerId).
/// </summary>
public class LeagueAccess : ILeagueAccess
{
    private readonly ICurrentUser _currentUser;
    private readonly IManagerRepository _managerRepository;
    private readonly ILeagueRepository _leagueRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeagueAccess"/> class.
    /// </summary>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="managerRepository">The repository used to find the caller's manager profile.</param>
    /// <param name="leagueRepository">The repository used to load the league and its owner.</param>
    public LeagueAccess(ICurrentUser currentUser, IManagerRepository managerRepository, ILeagueRepository leagueRepository)
    {
        _currentUser = currentUser;
        _managerRepository = managerRepository;
        _leagueRepository = leagueRepository;
    }

    /// <summary>
    /// Ensures the current user may manage the given league: they are an operator, or they own it.
    /// </summary>
    /// <param name="leagueId">The identifier of the league.</param>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the league's owner; a league with no owner can only be managed by an operator.</exception>
    public async Task EnsureCanManageLeagueAsync(int leagueId)
    {
        if (_currentUser.IsOperator)
        {
            return;
        }

        // A league with no owner, or a caller with no manager profile, never matches, so such a league stays operator-only.
        var manager = await _managerRepository.GetByUserIdAsync(_currentUser.UserId);
        var league = await _leagueRepository.GetByIdAsync(leagueId);

        if (manager is null || league?.OwnerManagerId != manager.Id)
        {
            throw new ForbiddenAccessException("Only the league's owner can manage this league.");
        }
    }
}
