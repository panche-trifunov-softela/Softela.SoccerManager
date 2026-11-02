using FluentValidation;
using FluentValidation.Results;
using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Services;

/// <summary>
/// Reads and writes a club's lineup for a match for every caller, so the save, the auto pick and the read share one
/// definition.
/// </summary>
public class MatchLineupService : IMatchLineupService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IMatchTeamStatisticRepository _matchTeamStatisticRepository;
    private readonly IFormationRepository _formationRepository;
    private readonly IFormationPositionRepository _formationPositionRepository;
    private readonly ILeagueTeamRepository _leagueTeamRepository;
    private readonly IMatchFormationPlayerPositionRepository _matchFormationPlayerPositionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchLineupService"/> class.
    /// </summary>
    /// <param name="matchRepository">The repository used to load the match.</param>
    /// <param name="matchTeamStatisticRepository">The repository used to find the clubs playing in the match.</param>
    /// <param name="formationRepository">The repository used to find the default formation.</param>
    /// <param name="formationPositionRepository">The repository used to load a formation's slots.</param>
    /// <param name="leagueTeamRepository">The repository used to load the club's squad.</param>
    /// <param name="matchFormationPlayerPositionRepository">The repository used to read and write the club's lineup rows.</param>
    public MatchLineupService(IMatchRepository matchRepository, IMatchTeamStatisticRepository matchTeamStatisticRepository, IFormationRepository formationRepository, IFormationPositionRepository formationPositionRepository, ILeagueTeamRepository leagueTeamRepository, IMatchFormationPlayerPositionRepository matchFormationPlayerPositionRepository)
    {
        _matchRepository = matchRepository;
        _matchTeamStatisticRepository = matchTeamStatisticRepository;
        _formationRepository = formationRepository;
        _formationPositionRepository = formationPositionRepository;
        _leagueTeamRepository = leagueTeamRepository;
        _matchFormationPlayerPositionRepository = matchFormationPlayerPositionRepository;
    }

    /// <summary>
    /// Ensures the club plays in the match. It runs for every caller, operators included, because
    /// <see cref="IClubAccess.EnsureCanManageMatchAsync"/> skips these checks for operators; a lineup is only ever written for
    /// a club that plays the match.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose lineup is being read or written.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the match does not exist.</exception>
    /// <exception cref="ValidationException">Thrown when the club is not one of the match's participants.</exception>
    public async Task EnsureClubPlaysMatchAsync(int matchId, int leagueTeamId)
    {
        _ = await _matchRepository.GetByIdAsync(matchId)
            ?? throw new KeyNotFoundException($"Match {matchId} not found.");

        var teamStatistics = await _matchTeamStatisticRepository.GetByMatchIdAsync(matchId);

        if (!teamStatistics.Any(teamStatistic => teamStatistic.LeagueTeamId == leagueTeamId))
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(MatchFormationPlayerPosition.MatchId), "The club does not play in this match."),
            });
        }
    }

    /// <summary>
    /// Finds the formation an auto pick uses when nothing names one, and the one an empty lineup is shown in.
    /// </summary>
    /// <returns>The identifier of the default formation, or <see langword="null"/> when no formation has all eleven starter slots.</returns>
    public async Task<int?> GetDefaultFormationIdAsync()
    {
        var formation = await _formationRepository.GetDefaultForLineupAsync(LineupAutoPicker.DefaultFormationName);

        return formation?.Id;
    }

    /// <summary>
    /// Loads a club's squad as lineup candidates, suspended and position-less players included.
    /// </summary>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose squad to load.</param>
    /// <returns>The squad's players by player identifier.</returns>
    public async Task<IReadOnlyDictionary<int, LineupCandidate>> GetSquadAsync(int leagueTeamId)
    {
        var rows = await _leagueTeamRepository.GetLineupCandidatesAsync(leagueTeamId);

        // The rows arrive grouped by player, best rating first, and the player's own columns repeat on every row.
        return rows
            .GroupBy(row => row.PlayerId)
            .ToDictionary(group => group.Key, group => ToCandidate(group.ToList()));
    }

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
    public async Task InsertAsync(int matchId, int leagueTeamId, IReadOnlyList<GetFormationSlotsResult> slots, IReadOnlyDictionary<int, LineupCandidate> squad, IReadOnlyDictionary<int, int> assignments, DateTime now, Guid userId)
    {
        foreach (var (slotNumber, playerId) in assignments.OrderBy(assignment => assignment.Key))
        {
            var slot = slots.First(formationSlot => formationSlot.SlotNumber == slotNumber);
            var candidate = squad[playerId];

            // A starter slot uses the player's rating for its position when he has one; otherwise, and on the bench, his main rating.
            var rating = candidate.RowFor(slot.PositionId) ?? candidate.Main!;

            var lineupRow = new MatchFormationPlayerPosition
            {
                MatchId = matchId,
                LeagueTeamId = leagueTeamId,
                FormationPositionId = slot.FormationPositionId,
                PlayerPositionId = rating.PlayerPositionId,
                ConditionOnMatch = candidate.Condition,
                QualityAtPositionOnMatch = rating.Quality,
                IsSuspended = candidate.IsSuspended,
                CreatedAt = now,
                ModifiedAt = now,
                CreatedBy = userId,
                ModifiedBy = userId,
            };

            await _matchFormationPlayerPositionRepository.CreateAsync(lineupRow);
        }
    }

    /// <summary>
    /// Builds a club's lineup for a match: every slot of its formation, filled or empty. The formation is the one of the club's
    /// first row; rows of any other formation are ignored.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose lineup to build.</param>
    /// <param name="formationIdWhenEmpty">The formation to show when the club has no rows, or <see langword="null"/> for the default one.</param>
    /// <returns>The lineup, with no formation name and no slots when the formation has no slots, and no formation at all when none is stored and no default exists.</returns>
    public async Task<MatchLineupDto> GetLineupAsync(int matchId, int leagueTeamId, int? formationIdWhenEmpty)
    {
        var rows = await _matchFormationPlayerPositionRepository.GetLineupRowsAsync(matchId, leagueTeamId);

        // The rows come in slot order, so the first one names the lineup's formation. The default formation is read only for an
        // empty lineup that names none.
        var formationId = rows.Count > 0 ? rows[0].FormationId : formationIdWhenEmpty ?? await GetDefaultFormationIdAsync();

        if (formationId is null)
        {
            return new MatchLineupDto
            {
                MatchId = matchId,
                LeagueTeamId = leagueTeamId,
                FormationId = null,
                FormationName = null,
                Slots = [],
            };
        }

        var slots = await _formationPositionRepository.GetSlotsAsync(formationId.Value);

        // Only the rows of the lineup's formation are shown; the first of two rows in one slot wins.
        var rowsBySlot = new Dictionary<int, GetMatchLineupRowsResult>();

        foreach (var row in rows.Where(row => row.FormationId == formationId))
        {
            rowsBySlot.TryAdd(row.SlotNumber, row);
        }

        return new MatchLineupDto
        {
            MatchId = matchId,
            LeagueTeamId = leagueTeamId,
            FormationId = formationId,
            FormationName = slots.Count > 0 ? slots[0].FormationName : null,
            Slots = slots.Select(slot => ToSlotDto(slot, rowsBySlot.GetValueOrDefault(slot.SlotNumber))).ToList(),
        };
    }

    private static LineupCandidate ToCandidate(IReadOnlyList<GetLeagueTeamLineupCandidatesResult> playerRows)
    {
        var first = playerRows[0];

        return new LineupCandidate
        {
            PlayerId = first.PlayerId,
            SquadNumber = first.SquadNumber,
            Condition = first.Condition,
            IsSuspended = first.IsSuspended,
            Rating = first.Rating,
            Positions = playerRows.Select(ToPosition).OfType<LineupCandidatePosition>().ToList(),
        };
    }

    private static LineupCandidatePosition? ToPosition(GetLeagueTeamLineupCandidatesResult row)
    {
        // A player with no position rating is one row with these columns null.
        if (row.PlayerPositionId is not int playerPositionId || row.PositionId is not int positionId || row.Quality is not int quality)
        {
            return null;
        }

        return new LineupCandidatePosition
        {
            PlayerPositionId = playerPositionId,
            PositionId = positionId,
            Quality = quality,
            IsGoalkeeper = row.PositionArea == PositionArea.Goalkeeper,
        };
    }

    private static MatchLineupSlotDto ToSlotDto(GetFormationSlotsResult slot, GetMatchLineupRowsResult? row)
    {
        return new MatchLineupSlotDto
        {
            SlotNumber = slot.SlotNumber,
            PositionId = slot.PositionId,
            PositionName = slot.PositionName,
            PositionShortName = slot.PositionShortName,
            PitchX = slot.PitchX,
            PitchY = slot.PitchY,
            PlayerId = row?.PlayerId,
            PlayerPositionId = row?.PlayerPositionId,
            PlayedPositionId = row?.PlayedPositionId,
            ConditionOnMatch = row?.ConditionOnMatch,
            QualityAtPositionOnMatch = row?.QualityAtPositionOnMatch,
            IsSuspended = row?.IsSuspended,
        };
    }
}
