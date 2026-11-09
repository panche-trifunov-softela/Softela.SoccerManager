using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="MatchTeamTacticSubstitution"/> entities.
/// </summary>
public interface IMatchTeamTacticSubstitutionRepository
{
    /// <summary>
    /// Persists a new match team tactic substitution and returns its generated identifier.
    /// </summary>
    /// <param name="matchTeamTacticSubstitution">The match team tactic substitution to create.</param>
    /// <returns>The identifier assigned to the created match team tactic substitution.</returns>
    Task<int> CreateAsync(MatchTeamTacticSubstitution matchTeamTacticSubstitution);

    /// <summary>
    /// Retrieves a club's substitution tactics for a match, ordered by minute, then situation.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league).</param>
    /// <returns>A list of the club's substitution tactics, empty when it has none.</returns>
    Task<List<MatchTeamTacticSubstitution>> GetByMatchAndLeagueTeamAsync(int matchId, int leagueTeamId);

    /// <summary>
    /// Retrieves the minute a club's substitutions for a match share. It locks the club's substitutions for the match until
    /// the current transaction ends, so a concurrent save of the same club waits for it; it must be called inside a
    /// transaction.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league).</param>
    /// <returns>The shared minute, or <see langword="null"/> when the club has no substitutions for the match.</returns>
    Task<int?> GetMinuteForUpdateAsync(int matchId, int leagueTeamId);

    /// <summary>
    /// Changes only the given instructions of a substitution tactic, leaving the others, the roles and the creation fields as
    /// they are.
    /// </summary>
    /// <param name="id">The identifier of the substitution tactic.</param>
    /// <param name="mentality">The new mentality, or <see langword="null"/> to keep it.</param>
    /// <param name="tempo">The new tempo, or <see langword="null"/> to keep it.</param>
    /// <param name="passing">The new passing style, or <see langword="null"/> to keep it.</param>
    /// <param name="width">The new width, or <see langword="null"/> to keep it.</param>
    /// <param name="pressing">The new pressing, or <see langword="null"/> to keep it.</param>
    /// <param name="tackling">The new tackling, or <see langword="null"/> to keep it.</param>
    /// <param name="attackingSide">The new attacking side, or <see langword="null"/> to keep it.</param>
    /// <param name="modifiedAt">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="modifiedBy">The identifier of the user making the change.</param>
    Task UpdateInstructionsAsync(int id, Mentality? mentality, Tempo? tempo, Passing? passing, Width? width, Pressing? pressing, Tackling? tackling, AttackingSide? attackingSide, DateTime modifiedAt, Guid modifiedBy);

    /// <summary>
    /// Sets the four roles of a substitution tactic.
    /// </summary>
    /// <param name="id">The identifier of the substitution tactic.</param>
    /// <param name="penaltyTakerPlayerId">The identifier of the player who takes penalties.</param>
    /// <param name="freeKickTakerPlayerId">The identifier of the player who takes free kicks.</param>
    /// <param name="cornerKickTakerPlayerId">The identifier of the player who takes corner kicks.</param>
    /// <param name="captainPlayerId">The identifier of the player who captains the team.</param>
    /// <param name="modifiedAt">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="modifiedBy">The identifier of the user making the change.</param>
    Task UpdateRolesAsync(int id, int penaltyTakerPlayerId, int freeKickTakerPlayerId, int cornerKickTakerPlayerId, int captainPlayerId, DateTime modifiedAt, Guid modifiedBy);

    /// <summary>
    /// Removes a club's substitution tactic for a match at a minute and situation. No matching row is not an error.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league).</param>
    /// <param name="minuteAt">The minute of the substitution to remove.</param>
    /// <param name="situation">The situation of the substitution to remove.</param>
    Task DeleteAsync(int matchId, int leagueTeamId, int minuteAt, Situation situation);
}
