using FluentValidation;
using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Services;

/// <summary>
/// Reads and creates a club's substitutions for a match, so the saves, the delete and the read share one definition.
/// </summary>
public interface IMatchSubstitutionService
{
    /// <summary>
    /// Retrieves a club's main lineup rows for a match as its lineup shows them: the rows of the first row's formation, one
    /// per slot (the first of two rows in one slot wins), in slot order; empty when the club has no lineup.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose main lineup to read.</param>
    /// <returns>The club's main lineup rows in slot order.</returns>
    Task<IReadOnlyList<GetMatchLineupRowsResult>> GetMainLineupRowsAsync(int matchId, int leagueTeamId);

    /// <summary>
    /// Finds a club's substitution for a match at a minute and situation, creating it when it does not exist yet: the lineup as
    /// a copy of the main lineup's rows, the instructions the main lineup shows, and the main tactic's roles, else roles
    /// picked from the main starters. It must run inside the caller's transaction, which it locks against concurrent saves of
    /// the same club for the match.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) the substitution is for.</param>
    /// <param name="minuteAt">The minute of the match the substitution takes effect at.</param>
    /// <param name="situation">The state of the score the substitution is for.</param>
    /// <param name="now">The UTC moment stamped on anything created.</param>
    /// <param name="userId">The identifier of the user stamped on anything created.</param>
    /// <returns>The substitution's tactic, existing or new.</returns>
    /// <exception cref="ValidationException">Thrown when the club already has substitutions for the match at another minute, or when a new substitution finds fewer than 11 starters in the main lineup.</exception>
    Task<MatchTeamTacticSubstitution> EnsureSubstitutionAsync(int matchId, int leagueTeamId, int minuteAt, Situation situation, DateTime now, Guid userId);

    /// <summary>
    /// Builds a club's saved substitutions for a match, ordered by minute, then situation. Each lineup slot says how the
    /// substitution moves its player compared with the main starters: coming on for a starter who is not a main starter, going
    /// off for a reserve who is one. It never writes.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose substitutions to build.</param>
    /// <returns>The club's substitutions for the match; empty when it has none.</returns>
    Task<IReadOnlyList<MatchSubstitutionDto>> GetSubstitutionsAsync(int matchId, int leagueTeamId);
}
