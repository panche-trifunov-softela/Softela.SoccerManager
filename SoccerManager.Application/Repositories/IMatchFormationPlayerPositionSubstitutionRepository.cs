using SoccerManager.Application.Models;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="MatchFormationPlayerPositionSubstitution"/> entities.
/// </summary>
public interface IMatchFormationPlayerPositionSubstitutionRepository
{
    /// <summary>
    /// Persists a new match formation player position substitution and returns its generated identifier.
    /// </summary>
    /// <param name="matchFormationPlayerPositionSubstitution">The match formation player position substitution to create.</param>
    /// <returns>The identifier assigned to the created match formation player position substitution.</returns>
    Task<int> CreateAsync(MatchFormationPlayerPositionSubstitution matchFormationPlayerPositionSubstitution);

    /// <summary>
    /// Removes a match formation player position substitution by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match formation player position substitution to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Removes the whole lineup of one of a club's substitutions for a match.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose substitution lineup to remove.</param>
    /// <param name="minuteAt">The minute of the substitution whose lineup to remove.</param>
    /// <param name="situation">The situation of the substitution whose lineup to remove.</param>
    Task DeleteByMinuteAndSituationAsync(int matchId, int leagueTeamId, int minuteAt, Situation situation);

    /// <summary>
    /// Removes a player's slots for a club from its substitution lineups in matches that have not started. Slots in started
    /// matches are kept as the record of what was planned.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league roster entry (the club in its league) whose slots to remove.</param>
    /// <param name="playerId">The identifier of the player whose slots to remove.</param>
    Task DeleteUnstartedByLeagueTeamAndPlayerAsync(int leagueTeamId, int playerId);

    /// <summary>
    /// Retrieves every substitution lineup row of a club for a match, each joined to its formation slot and to the player
    /// behind its position rating, ordered by minute, situation, then slot.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose substitution lineups to retrieve.</param>
    /// <returns>A list of the club's substitution lineup rows, empty when it has none.</returns>
    Task<List<GetMatchLineupSubstitutionRowsResult>> GetLineupRowsAsync(int matchId, int leagueTeamId);
}
