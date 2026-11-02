using FluentValidation;
using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Services;

/// <summary>
/// Reads and writes a club's lineup for a match for every caller, so the save, the auto pick and the read share one
/// definition.
/// </summary>
public interface IMatchLineupService
{
    /// <summary>
    /// Ensures the club plays in the match. It runs for every caller, operators included, because
    /// <see cref="IClubAccess.EnsureCanManageMatchAsync"/> skips these checks for operators; a lineup is only ever written for
    /// a club that plays the match.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose lineup is being read or written.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club is not one of the match's participants.</exception>
    Task EnsureClubPlaysMatchAsync(int matchId, int leagueTeamId);

    /// <summary>
    /// Finds the formation an auto pick uses when nothing names one, and the one an empty lineup is shown in.
    /// </summary>
    /// <returns>The identifier of the default formation, or <see langword="null"/> when no formation has all eleven starter slots.</returns>
    Task<int?> GetDefaultFormationIdAsync();

    /// <summary>
    /// Loads a club's squad as lineup candidates, suspended and position-less players included.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose squad to load.</param>
    /// <returns>The squad's players by player identifier.</returns>
    Task<IReadOnlyDictionary<int, LineupCandidate>> GetSquadAsync(int leagueTeamId);

    /// <summary>
    /// Writes lineup rows for the given assignments, snapshotting each player's condition, the quality of the rating used and
    /// suspension from the squad. Every assigned player must be in the squad with position data.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) the rows are for.</param>
    /// <param name="slots">The slots of the formation the assignments refer to.</param>
    /// <param name="squad">The club's squad, from <see cref="GetSquadAsync"/>.</param>
    /// <param name="assignments">The identifiers of the players to seat, by slot number.</param>
    /// <param name="now">The UTC moment stamped on the rows.</param>
    /// <param name="userId">The identifier of the user stamped on the rows.</param>
    Task InsertAsync(int matchId, int leagueTeamId, IReadOnlyList<GetFormationSlotsResult> slots, IReadOnlyDictionary<int, LineupCandidate> squad, IReadOnlyDictionary<int, int> assignments, DateTime now, Guid userId);

    /// <summary>
    /// Builds a club's lineup for a match: every slot of its formation, filled or empty. The formation is the one of the club's
    /// first row; rows of any other formation are ignored.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose lineup to build.</param>
    /// <param name="formationIdWhenEmpty">The formation to show when the club has no rows, or <see langword="null"/> for the default one.</param>
    /// <returns>The lineup, with no formation name and no slots when the formation has no slots, and no formation at all when none is stored and no default exists.</returns>
    Task<MatchLineupDto> GetLineupAsync(int matchId, int leagueTeamId, int? formationIdWhenEmpty);
}
