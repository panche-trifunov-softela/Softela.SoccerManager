using FluentValidation;
using FluentValidation.Results;
using SoccerManager.Application.Commands.Match.SaveMatchInstructions;
using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;
using SoccerManager.Application.Repositories;
using SoccerManager.Domain.Entities;
using SoccerManager.Domain.Enums;

namespace SoccerManager.Application.Services;

/// <summary>
/// Reads and creates a club's substitutions for a match, so the saves, the delete and the read share one definition.
/// </summary>
public class MatchSubstitutionService : IMatchSubstitutionService
{
    private readonly IMatchLineupService _lineupService;
    private readonly IMatchFormationPlayerPositionRepository _matchFormationPlayerPositionRepository;
    private readonly IMatchTeamTacticRepository _matchTeamTacticRepository;
    private readonly IMatchTeamTacticSubstitutionRepository _matchTeamTacticSubstitutionRepository;
    private readonly IMatchFormationPlayerPositionSubstitutionRepository _matchFormationPlayerPositionSubstitutionRepository;
    private readonly IFormationPositionRepository _formationPositionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="MatchSubstitutionService"/> class.
    /// </summary>
    /// <param name="lineupService">The service used to build lineups and load the squad.</param>
    /// <param name="matchFormationPlayerPositionRepository">The repository used to read the club's main lineup rows.</param>
    /// <param name="matchTeamTacticRepository">The repository used to read the club's main instructions and roles.</param>
    /// <param name="matchTeamTacticSubstitutionRepository">The repository used to read and write the club's substitution tactics.</param>
    /// <param name="matchFormationPlayerPositionSubstitutionRepository">The repository used to read and write the club's substitution lineup rows.</param>
    /// <param name="formationPositionRepository">The repository used to load a formation's slots.</param>
    public MatchSubstitutionService(IMatchLineupService lineupService, IMatchFormationPlayerPositionRepository matchFormationPlayerPositionRepository, IMatchTeamTacticRepository matchTeamTacticRepository, IMatchTeamTacticSubstitutionRepository matchTeamTacticSubstitutionRepository, IMatchFormationPlayerPositionSubstitutionRepository matchFormationPlayerPositionSubstitutionRepository, IFormationPositionRepository formationPositionRepository)
    {
        _lineupService = lineupService;
        _matchFormationPlayerPositionRepository = matchFormationPlayerPositionRepository;
        _matchTeamTacticRepository = matchTeamTacticRepository;
        _matchTeamTacticSubstitutionRepository = matchTeamTacticSubstitutionRepository;
        _matchFormationPlayerPositionSubstitutionRepository = matchFormationPlayerPositionSubstitutionRepository;
        _formationPositionRepository = formationPositionRepository;
    }

    /// <summary>
    /// Retrieves a club's main lineup rows for a match as its lineup shows them: the rows of the first row's formation, one
    /// per slot (the first of two rows in one slot wins), in slot order; empty when the club has no lineup.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose main lineup to read.</param>
    /// <returns>The club's main lineup rows in slot order.</returns>
    public async Task<IReadOnlyList<GetMatchLineupRowsResult>> GetMainLineupRowsAsync(int matchId, int leagueTeamId)
    {
        var rows = await _matchFormationPlayerPositionRepository.GetLineupRowsAsync(matchId, leagueTeamId);

        if (rows.Count == 0)
        {
            return [];
        }

        // The rows come in slot order, so the first one names the lineup's formation, as MatchLineupService.GetLineupAsync reads it.
        var formationId = rows[0].FormationId;

        return rows
            .Where(row => row.FormationId == formationId)
            .GroupBy(row => row.SlotNumber)
            .Select(group => group.First())
            .ToList();
    }

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
    public async Task<MatchTeamTacticSubstitution> EnsureSubstitutionAsync(int matchId, int leagueTeamId, int minuteAt, Situation situation, DateTime now, Guid userId)
    {
        // Read first, under a lock held until the caller's transaction ends, so two first saves at different minutes run one
        // after the other and the second sees the first one's minute.
        var windowMinute = await _matchTeamTacticSubstitutionRepository.GetMinuteForUpdateAsync(matchId, leagueTeamId);

        var substitutions = await _matchTeamTacticSubstitutionRepository.GetByMatchAndLeagueTeamAsync(matchId, leagueTeamId);

        var existing = substitutions.FirstOrDefault(substitution => substitution.MinuteAt == minuteAt && substitution.Situation == situation);

        if (existing is not null)
        {
            return existing;
        }

        // A club's substitutions for a match share one minute, so moving them means deleting them first.
        if (windowMinute is int minute && minute != minuteAt)
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure("MinuteAt", $"Substitutions for this match are at minute {minute}. Delete those situations to use another minute."),
            });
        }

        var mainRows = await GetMainLineupRowsAsync(matchId, leagueTeamId);

        var starterCount = mainRows.Count(row => row.SlotNumber <= LineupAutoPicker.LastStarterSlot);

        if (starterCount < LineupAutoPicker.LastStarterSlot)
        {
            throw new ValidationException(new[] { new ValidationFailure("LeagueTeamId", "Pick 11 starters first.") });
        }

        // The lineup is a copy of the main one, snapshots included, so the substitution starts from the main lineup as it stands.
        foreach (var mainRow in mainRows)
        {
            await _matchFormationPlayerPositionSubstitutionRepository.CreateAsync(new MatchFormationPlayerPositionSubstitution
            {
                MatchId = matchId,
                LeagueTeamId = leagueTeamId,
                MinuteAt = minuteAt,
                Situation = situation,
                FormationPositionId = mainRow.FormationPositionId,
                PlayerPositionId = mainRow.PlayerPositionId,
                ConditionOnMatch = mainRow.ConditionOnMatch,
                QualityAtPositionOnMatch = mainRow.QualityAtPositionOnMatch,
                IsSuspended = mainRow.IsSuspended,
                CreatedAt = now,
                ModifiedAt = now,
                CreatedBy = userId,
                ModifiedBy = userId,
            });
        }

        var instructions = await _matchTeamTacticRepository.GetInstructionsAsync(matchId, leagueTeamId);

        var roles = await GetMainRolesAsync(matchId, leagueTeamId);

        var tactic = new MatchTeamTacticSubstitution
        {
            MatchId = matchId,
            LeagueTeamId = leagueTeamId,
            MinuteAt = minuteAt,
            Situation = situation,
            Mentality = instructions.Mentality,
            Tempo = instructions.Tempo,
            Passing = instructions.Passing,
            Width = instructions.Width,
            Pressing = instructions.Pressing,
            Tackling = instructions.Tackling,
            AttackingSide = instructions.AttackingSide,
            PenaltyTakerPlayerId = roles.PenaltyTakerPlayerId,
            FreeKickTakerPlayerId = roles.FreeKickTakerPlayerId,
            CornerKickTakerPlayerId = roles.CornerKickTakerPlayerId,
            CaptainPlayerId = roles.CaptainPlayerId,
            CreatedAt = now,
            ModifiedAt = now,
            CreatedBy = userId,
            ModifiedBy = userId,
        };

        tactic.Id = await _matchTeamTacticSubstitutionRepository.CreateAsync(tactic);

        return tactic;
    }

    /// <summary>
    /// Builds a club's saved substitutions for a match, ordered by minute, then situation. Each lineup slot says how the
    /// substitution moves its player compared with the main starters: coming on for a starter who is not a main starter, going
    /// off for a reserve who is one. It never writes.
    /// </summary>
    /// <param name="matchId">The identifier of the match.</param>
    /// <param name="leagueTeamId">The identifier of the league team (the club in its league) whose substitutions to build.</param>
    /// <returns>The club's substitutions for the match; empty when it has none.</returns>
    public async Task<IReadOnlyList<MatchSubstitutionDto>> GetSubstitutionsAsync(int matchId, int leagueTeamId)
    {
        var tactics = await _matchTeamTacticSubstitutionRepository.GetByMatchAndLeagueTeamAsync(matchId, leagueTeamId);

        if (tactics.Count == 0)
        {
            return [];
        }

        var rows = await _matchFormationPlayerPositionSubstitutionRepository.GetLineupRowsAsync(matchId, leagueTeamId);

        var mainRows = await GetMainLineupRowsAsync(matchId, leagueTeamId);

        var mainStarters = mainRows
            .Where(row => row.SlotNumber <= LineupAutoPicker.LastStarterSlot)
            .Select(row => row.PlayerId)
            .ToHashSet();

        var substitutions = new List<MatchSubstitutionDto>();

        // The tactics arrive ordered by minute, then situation, the order the list keeps.
        foreach (var tactic in tactics)
        {
            var substitutionRows = rows
                .Where(row => row.MinuteAt == tactic.MinuteAt && row.Situation == tactic.Situation)
                .ToList<GetMatchLineupRowsResult>();

            var lineup = await _lineupService.BuildLineupAsync(matchId, leagueTeamId, substitutionRows, formationIdWhenEmpty: null);

            substitutions.Add(new MatchSubstitutionDto
            {
                MinuteAt = tactic.MinuteAt,
                Situation = tactic.Situation,
                Lineup = lineup with { Slots = lineup.Slots.Select(slot => slot with { Substitution = DirectionOf(slot, mainStarters) }).ToList() },
                Instructions = ToInstructionsDto(tactic),
            });
        }

        return substitutions;
    }

    // A new substitution takes the main tactic's roles. Before the club's first instructions save there is no main tactic row,
    // so the roles are picked from the main starters the way that first save picks them.
    private async Task<(int CaptainPlayerId, int PenaltyTakerPlayerId, int FreeKickTakerPlayerId, int CornerKickTakerPlayerId)> GetMainRolesAsync(int matchId, int leagueTeamId)
    {
        var roles = await _matchTeamTacticRepository.GetRolesAsync(matchId, leagueTeamId);

        if (roles is not null)
        {
            return (roles.CaptainPlayerId, roles.PenaltyTakerPlayerId, roles.FreeKickTakerPlayerId, roles.CornerKickTakerPlayerId);
        }

        var lineup = await _lineupService.GetLineupAsync(matchId, leagueTeamId, formationIdWhenEmpty: null);

        var formationSlots = await _formationPositionRepository.GetSlotsAsync(lineup.FormationId!.Value);

        var squad = await _lineupService.GetSquadAsync(leagueTeamId);

        return SaveMatchInstructionsRolePicker.Pick(lineup.Slots, formationSlots, squad);
    }

    // Compared with the main starters, not the main lineup's slots, so two starters swapping places get no marker.
    private static SubstitutionDirection? DirectionOf(MatchLineupSlotDto slot, HashSet<int> mainStarters)
    {
        if (slot.PlayerId is not int playerId)
        {
            return null;
        }

        var isStarterSlot = slot.SlotNumber <= LineupAutoPicker.LastStarterSlot;

        if (isStarterSlot && !mainStarters.Contains(playerId))
        {
            return SubstitutionDirection.ComingOn;
        }

        if (!isStarterSlot && mainStarters.Contains(playerId))
        {
            return SubstitutionDirection.GoingOff;
        }

        return null;
    }

    private static MatchInstructionsDto ToInstructionsDto(MatchTeamTacticSubstitution tactic)
    {
        return new MatchInstructionsDto
        {
            MatchId = tactic.MatchId,
            LeagueTeamId = tactic.LeagueTeamId,
            Mentality = tactic.Mentality,
            Tempo = tactic.Tempo,
            Passing = tactic.Passing,
            Width = tactic.Width,
            Pressing = tactic.Pressing,
            Tackling = tactic.Tackling,
            AttackingSide = tactic.AttackingSide,
        };
    }
}
