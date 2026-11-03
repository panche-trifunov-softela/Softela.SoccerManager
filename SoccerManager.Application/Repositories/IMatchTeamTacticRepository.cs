using SoccerManager.Application.Models;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="MatchTeamTactic"/> entities.
/// </summary>
public interface IMatchTeamTacticRepository
{
    /// <summary>
    /// Persists a new match team tactic and returns its generated identifier.
    /// </summary>
    /// <param name="matchTeamTactic">The match team tactic to create.</param>
    /// <returns>The identifier assigned to the created match team tactic.</returns>
    Task<int> CreateAsync(MatchTeamTactic matchTeamTactic);

    /// <summary>
    /// Persists changes to an existing match team tactic.
    /// </summary>
    /// <param name="matchTeamTactic">The match team tactic with updated values.</param>
    /// <returns>The identifier of the updated match team tactic.</returns>
    Task<int> UpdateAsync(MatchTeamTactic matchTeamTactic);

    /// <summary>
    /// Removes a match team tactic by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team tactic to delete.</param>
    Task DeleteAsync(int id);

    /// <summary>
    /// Retrieves a match team tactic by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the match team tactic to retrieve.</param>
    /// <returns>The matching match team tactic, or <see langword="null"/> when none exists.</returns>
    Task<MatchTeamTactic?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves every team tactic recorded for a match.
    /// </summary>
    /// <param name="matchId">The identifier of the match whose team tactics to retrieve.</param>
    /// <returns>A list of every team tactic recorded for the match.</returns>
    Task<List<MatchTeamTactic>> GetByMatchIdAsync(int matchId);

    /// <summary>
    /// Retrieves a club's seven instructions for a match: the ones saved for it, else the ones the club last saved for an
    /// earlier match, else the defaults. It never writes.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league).</param>
    /// <returns>The club's instructions for the match.</returns>
    Task<GetMatchTeamTacticInstructionsResult> GetInstructionsAsync(int matchId, int leagueTeamId);

    /// <summary>
    /// Changes only the given instructions of a club's tactic for a match, leaving the others, the roles and the creation
    /// fields as they are.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league).</param>
    /// <param name="mentality">The new mentality, or <see langword="null"/> to keep it.</param>
    /// <param name="tempo">The new tempo, or <see langword="null"/> to keep it.</param>
    /// <param name="passing">The new passing style, or <see langword="null"/> to keep it.</param>
    /// <param name="width">The new width, or <see langword="null"/> to keep it.</param>
    /// <param name="pressing">The new pressing, or <see langword="null"/> to keep it.</param>
    /// <param name="tackling">The new tackling, or <see langword="null"/> to keep it.</param>
    /// <param name="attackingSide">The new attacking side, or <see langword="null"/> to keep it.</param>
    /// <param name="modifiedAt">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="modifiedBy">The identifier of the user making the change.</param>
    /// <returns>The club's instructions after the change, or <see langword="null"/> when the club has no tactic for the match.</returns>
    Task<GetMatchTeamTacticInstructionsResult?> UpdateInstructionsAsync(int matchId, int leagueTeamId, Mentality? mentality, Tempo? tempo, Passing? passing, Width? width, Pressing? pressing, Tackling? tackling, AttackingSide? attackingSide, DateTime modifiedAt, Guid modifiedBy);
}
