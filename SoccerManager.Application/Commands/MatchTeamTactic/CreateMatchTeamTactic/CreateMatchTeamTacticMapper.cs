namespace SoccerManager.Application.Commands.MatchTeamTactic.CreateMatchTeamTactic;

/// <summary>
/// Maps <see cref="CreateMatchTeamTacticRequest"/> instances to domain entities.
/// </summary>
public static class CreateMatchTeamTacticMapper
{
    /// <summary>
    /// Creates a new match team tactic entity from the given request.
    /// </summary>
    /// <param name="request">The request carrying the match team tactic values.</param>
    /// <param name="now">The UTC timestamp to stamp on the created and modified fields.</param>
    /// <param name="userId">The identifier of the user creating the match team tactic.</param>
    /// <returns>A new, unsaved match team tactic entity.</returns>
    public static SoccerManager.Domain.Entities.MatchTeamTactic ToDomainEntity(CreateMatchTeamTacticRequest request, DateTime now, Guid userId)
    {
        return new SoccerManager.Domain.Entities.MatchTeamTactic
        {
            MatchId = request.MatchId,
            TeamId = request.TeamId,
            Mentality = request.Mentality,
            Tempo = request.Tempo,
            Passing = request.Passing,
            Width = request.Width,
            Pressing = request.Pressing,
            Tackling = request.Tackling,
            AttackingSide = request.AttackingSide,
            PenaltyTakerPlayerId = request.PenaltyTakerPlayerId,
            FreeKickTakerPlayerId = request.FreeKickTakerPlayerId,
            CornerKickTakerPlayerId = request.CornerKickTakerPlayerId,
            CaptainPlayerId = request.CaptainPlayerId,
            CreatedAt = now,
            CreatedBy = userId,
            ModifiedAt = now,
            ModifiedBy = userId,
        };
    }
}
