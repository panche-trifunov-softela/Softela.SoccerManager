using FluentValidation;
using FluentValidation.Results;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;

namespace SoccerManager.Application.Services;

/// <summary>
/// Resolves the values a match formation player position copies from the match, the player's position rating
/// and the player's league registration.
/// </summary>
public class MatchFormationPlayerPositionSnapshotResolver : IMatchFormationPlayerPositionSnapshotResolver
{
    private readonly IMatchRepository _matchRepository;
    private readonly ISeasonRepository _seasonRepository;
    private readonly IPlayerPositionRepository _playerPositionRepository;
    private readonly ILeagueTeamPlayerRepository _leagueTeamPlayerRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchFormationPlayerPositionSnapshotResolver"/> class.
    /// </summary>
    /// <param name="matchRepository">The repository used to load the match.</param>
    /// <param name="seasonRepository">The repository used to reach the match's league through its season.</param>
    /// <param name="playerPositionRepository">The repository used to load the player's position rating.</param>
    /// <param name="leagueTeamPlayerRepository">The repository used to load the player's registration in the league.</param>
    public MatchFormationPlayerPositionSnapshotResolver(IMatchRepository matchRepository, ISeasonRepository seasonRepository, IPlayerPositionRepository playerPositionRepository, ILeagueTeamPlayerRepository leagueTeamPlayerRepository)
    {
        _matchRepository = matchRepository;
        _seasonRepository = seasonRepository;
        _playerPositionRepository = playerPositionRepository;
        _leagueTeamPlayerRepository = leagueTeamPlayerRepository;
    }

    /// <summary>
    /// Loads the player's league registration and position rating for the given match and team and returns
    /// the values to snapshot.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="teamId">The identifier of the team the player is lining up for.</param>
    /// <param name="playerPositionId">The identifier of the player position rating filling the slot.</param>
    /// <returns>The values to copy onto the match formation player position.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the match, its season, the player position or the player's registration in the match's league does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the player is registered with a different team in the match's league.</exception>
    public async Task<MatchFormationPlayerPositionSnapshot> ResolveAsync(int matchId, int teamId, int playerPositionId)
    {
        // Each link is checked explicitly so a missing one surfaces as a 404 rather than an unhandled 500.
        var match = await _matchRepository.GetByIdAsync(matchId)
            ?? throw new KeyNotFoundException($"Match {matchId} not found.");

        // A match carries only its season; the league the registration lives in is reached through it.
        var season = await _seasonRepository.GetByIdAsync(match.SeasonId)
            ?? throw new KeyNotFoundException($"Season {match.SeasonId} not found.");

        var playerPosition = await _playerPositionRepository.GetByIdAsync(playerPositionId)
            ?? throw new KeyNotFoundException($"Player position {playerPositionId} not found.");

        var leagueTeamPlayer = await _leagueTeamPlayerRepository.GetByLeagueAndPlayerAsync(season.LeagueId, playerPosition.PlayerId)
            ?? throw new KeyNotFoundException($"Player {playerPosition.PlayerId} is not registered in league {season.LeagueId}.");

        // The database cannot check this: the registration's team is reached through the player, not the request.
        if (leagueTeamPlayer.TeamId != teamId)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(MatchFormationPlayerPosition.TeamId), $"Player {playerPosition.PlayerId} is registered with team {leagueTeamPlayer.TeamId} in league {season.LeagueId}, not team {teamId}."),
            });
        }

        return new MatchFormationPlayerPositionSnapshot
        {
            ConditionOnMatch = leagueTeamPlayer.Condition,
            QualityAtPositionOnMatch = playerPosition.Quality,
            // Every match is domestic until Competitions exist: the model today has only leagues. The
            // continental flag is wired in here once a match knows which competition it belongs to.
            IsSuspended = leagueTeamPlayer.IsSuspendedDomesticCompetition,
        };
    }
}
