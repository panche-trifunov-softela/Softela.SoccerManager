using SoccerManager.Application.Dtos;

namespace SoccerManager.Application.Queries.MatchTeamTactic.GetMatchTeamTactics;

/// <summary>
/// Maps match team tactic domain entities to <see cref="MatchTeamTacticDto"/> instances.
/// </summary>
public static class GetMatchTeamTacticsMapper
{
    /// <summary>
    /// Converts a match team tactic entity into its DTO representation.
    /// </summary>
    /// <param name="matchTeamTactic">The match team tactic entity to convert.</param>
    /// <returns>The corresponding <see cref="MatchTeamTacticDto"/>.</returns>
    public static MatchTeamTacticDto ToDto(SoccerManager.Domain.Entities.MatchTeamTactic matchTeamTactic)
    {
        return new MatchTeamTacticDto
        {
            Id = matchTeamTactic.Id,
            MatchId = matchTeamTactic.MatchId,
            TeamId = matchTeamTactic.TeamId,
            Mentality = matchTeamTactic.Mentality,
            Tempo = matchTeamTactic.Tempo,
            Passing = matchTeamTactic.Passing,
            Width = matchTeamTactic.Width,
            Pressing = matchTeamTactic.Pressing,
            Tackling = matchTeamTactic.Tackling,
            AttackingSide = matchTeamTactic.AttackingSide,
            PenaltyTakerPlayerId = matchTeamTactic.PenaltyTakerPlayerId,
            FreeKickTakerPlayerId = matchTeamTactic.FreeKickTakerPlayerId,
            CornerKickTakerPlayerId = matchTeamTactic.CornerKickTakerPlayerId,
            CaptainPlayerId = matchTeamTactic.CaptainPlayerId,
            CreatedAt = matchTeamTactic.CreatedAt,
            ModifiedAt = matchTeamTactic.ModifiedAt,
        };
    }
}
