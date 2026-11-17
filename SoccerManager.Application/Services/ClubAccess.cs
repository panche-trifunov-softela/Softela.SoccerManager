using FluentValidation;
using FluentValidation.Results;
using SoccerManager.Application.Core.Exceptions;
using SoccerManager.Application.Core.User;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Services;

/// <summary>
/// Decides whether the current caller is an operator or the current manager of a club, whether the caller may view
/// a club's squad, and whether a match is still open to that club's changes.
/// </summary>
public class ClubAccess : IClubAccess
{
    private readonly ICurrentUser _currentUser;
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly ILeagueTeamManagerRepository _leagueTeamManagerRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IMatchTeamStatisticRepository _matchTeamStatisticRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClubAccess"/> class.
    /// </summary>
    /// <param name="currentUser">The currently authenticated user.</param>
    /// <param name="leagueTeamRepository">The repository used to load the club whose league is matched.</param>
    /// <param name="leagueTeamManagerRepository">The repository used to find the clubs the caller currently manages.</param>
    /// <param name="matchRepository">The repository used to load the match.</param>
    /// <param name="matchTeamStatisticRepository">The repository used to find the clubs playing in the match.</param>
    public ClubAccess(ICurrentUser currentUser, ILeagueTeamRepository leagueTeamRepository, ILeagueTeamManagerRepository leagueTeamManagerRepository, IMatchRepository matchRepository, IMatchTeamStatisticRepository matchTeamStatisticRepository)
    {
        _currentUser = currentUser;
        _leagueTeamRepository = leagueTeamRepository;
        _leagueTeamManagerRepository = leagueTeamManagerRepository;
        _matchRepository = matchRepository;
        _matchTeamStatisticRepository = matchTeamStatisticRepository;
    }

    /// <summary>
    /// Ensures the caller may view the given club: an operator always may, anyone else must be the current manager of
    /// a club in the same league as the given club, the given club or another one.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league roster entry (the club in its league) being viewed.</param>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is not an operator and manages no club in the club's league, or when the club does not exist.</exception>
    public async Task EnsureCanViewClubAsync(int leagueTeamId)
    {
        if (_currentUser.IsOperator)
        {
            return;
        }

        var appointments = await _leagueTeamManagerRepository.GetByUserIdAsync(_currentUser.UserId, currentOnly: true);

        if (appointments.Count == 0)
        {
            throw new ForbiddenAccessException("Only a current manager of a club in this club's league can view this club.");
        }

        // An unknown club has no league to match, so it is refused like a club of another league.
        var leagueTeam = await _leagueTeamRepository.GetByIdAsync(leagueTeamId);

        if (leagueTeam is null || !appointments.Any(appointment => appointment.LeagueId == leagueTeam.LeagueId))
        {
            throw new ForbiddenAccessException("Only a current manager of a club in this club's league can view this club.");
        }
    }

    /// <summary>
    /// Ensures the caller is an operator or the current manager of the given club.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league roster entry (the club in its league) being changed.</param>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    public async Task EnsureCanManageClubAsync(int leagueTeamId)
    {
        if (_currentUser.IsOperator)
        {
            return;
        }

        // One manager can be current at several clubs (one per league), so the club is looked up in the set.
        var appointments = await _leagueTeamManagerRepository.GetByUserIdAsync(_currentUser.UserId, currentOnly: true);

        if (!appointments.Any(appointment => appointment.LeagueTeamId == leagueTeamId))
        {
            throw new ForbiddenAccessException("Only the club's current manager can change this club.");
        }
    }

    /// <summary>
    /// Ensures the caller may change the given club's tactics or lineup for the given match: an operator
    /// always may, the club's current manager only while the club plays in a match that has not started.
    /// </summary>
    /// <param name="matchId">The identifier of the match being changed.</param>
    /// <param name="leagueTeamId">The identifier of the league roster entry (the club in its league) being changed.</param>
    /// <exception cref="ForbiddenAccessException">Thrown when the caller is neither an operator nor the club's current manager.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club does not play in the match or the match has already started.</exception>
    public async Task EnsureCanManageMatchAsync(int matchId, int leagueTeamId)
    {
        await EnsureCanManageClubAsync(leagueTeamId);

        if (_currentUser.IsOperator)
        {
            return;
        }

        var match = await _matchRepository.GetByIdAsync(matchId)
            ?? throw new KeyNotFoundException($"Match {matchId} not found.");

        var teamStatistics = await _matchTeamStatisticRepository.GetByMatchIdAsync(matchId);

        if (!teamStatistics.Any(teamStatistic => teamStatistic.LeagueTeamId == leagueTeamId))
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(MatchFormationPlayerPosition.MatchId), "The club does not play in this match."),
            });
        }

        // A started match is settled: its tactics and lineup are the record of how it was played.
        if (match.IsStarted)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(MatchFormationPlayerPosition.MatchId), "The match has already started."),
            });
        }
    }
}
