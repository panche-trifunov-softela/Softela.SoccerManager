using SoccerManager.Application.Dtos;
using SoccerManager.Application.Models;

namespace SoccerManager.Application.Commands.Match.SaveMatchInstructions;

/// <summary>
/// Maps <see cref="SaveMatchInstructionsRequest"/> instances to domain entities, and instructions to <see cref="MatchInstructionsDto"/> instances.
/// </summary>
public static class SaveMatchInstructionsMapper
{
    /// <summary>
    /// Creates a new match team tactic entity from the instructions the page showed, the changes in the request and the picked roles.
    /// </summary>
    /// <param name="request">The request carrying the changed instructions; one it does not send keeps the shown value.</param>
    /// <param name="shown">The instructions the page showed before the change.</param>
    /// <param name="roles">The captain and the set-piece takers picked for the club.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the match team tactic.</param>
    /// <returns>A new, unsaved match team tactic entity.</returns>
    public static SoccerManager.Domain.Entities.MatchTeamTactic ToDomainEntity(SaveMatchInstructionsRequest request, GetMatchTeamTacticInstructionsResult shown, (int CaptainPlayerId, int PenaltyTakerPlayerId, int FreeKickTakerPlayerId, int CornerKickTakerPlayerId) roles, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.MatchTeamTactic
        {
            MatchId = request.MatchId,
            LeagueTeamId = request.LeagueTeamId,
            Mentality = request.Mentality ?? shown.Mentality,
            Tempo = request.Tempo ?? shown.Tempo,
            Passing = request.Passing ?? shown.Passing,
            Width = request.Width ?? shown.Width,
            Pressing = request.Pressing ?? shown.Pressing,
            Tackling = request.Tackling ?? shown.Tackling,
            AttackingSide = request.AttackingSide ?? shown.AttackingSide,
            PenaltyTakerPlayerId = roles.PenaltyTakerPlayerId,
            FreeKickTakerPlayerId = roles.FreeKickTakerPlayerId,
            CornerKickTakerPlayerId = roles.CornerKickTakerPlayerId,
            CaptainPlayerId = roles.CaptainPlayerId,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }

    /// <summary>
    /// Converts the instructions a stored procedure returned into the DTO representation.
    /// </summary>
    /// <param name="matchId">The identifier of the match the instructions are for.</param>
    /// <param name="leagueTeamId">The identifier of the league team the instructions belong to.</param>
    /// <param name="instructions">The row returned by the stored procedure.</param>
    /// <returns>The corresponding <see cref="MatchInstructionsDto"/>.</returns>
    public static MatchInstructionsDto ToDto(int matchId, int leagueTeamId, GetMatchTeamTacticInstructionsResult instructions)
    {
        return new MatchInstructionsDto
        {
            MatchId = matchId,
            LeagueTeamId = leagueTeamId,
            Mentality = instructions.Mentality,
            Tempo = instructions.Tempo,
            Passing = instructions.Passing,
            Width = instructions.Width,
            Pressing = instructions.Pressing,
            Tackling = instructions.Tackling,
            AttackingSide = instructions.AttackingSide,
        };
    }

    /// <summary>
    /// Converts a match team tactic entity into the DTO representation, leaving out its roles.
    /// </summary>
    /// <param name="tactic">The match team tactic to convert.</param>
    /// <returns>The corresponding <see cref="MatchInstructionsDto"/>.</returns>
    public static MatchInstructionsDto ToDto(SoccerManager.Domain.Entities.MatchTeamTactic tactic)
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
