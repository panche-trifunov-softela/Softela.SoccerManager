namespace SoccerManager.Application.Commands.MatchTeamTactic.UpdateMatchTeamTactic;

/// <summary>
/// Applies <see cref="UpdateMatchTeamTacticRequest"/> values onto an existing domain entity.
/// </summary>
public static class UpdateMatchTeamTacticMapper
{
    /// <summary>
    /// Applies the request values to the given match team tactic, preserving its creation metadata.
    /// </summary>
    /// <param name="request">The request carrying the new match team tactic values.</param>
    /// <param name="matchTeamTactic">The loaded match team tactic entity to mutate.</param>
    /// <param name="now">The UTC timestamp to stamp on the modified field.</param>
    /// <param name="userId">The identifier of the user performing the update.</param>
    public static void ApplyTo(UpdateMatchTeamTacticRequest request, SoccerManager.Domain.Entities.MatchTeamTactic matchTeamTactic, DateTime now, Guid userId)
    {
        matchTeamTactic.MatchId = request.MatchId;
        matchTeamTactic.TeamId = request.TeamId;
        matchTeamTactic.Mentality = request.Mentality;
        matchTeamTactic.Tempo = request.Tempo;
        matchTeamTactic.Passing = request.Passing;
        matchTeamTactic.Width = request.Width;
        matchTeamTactic.Pressing = request.Pressing;
        matchTeamTactic.Tackling = request.Tackling;
        matchTeamTactic.AttackingSide = request.AttackingSide;
        matchTeamTactic.PenaltyTakerPlayerId = request.PenaltyTakerPlayerId;
        matchTeamTactic.FreeKickTakerPlayerId = request.FreeKickTakerPlayerId;
        matchTeamTactic.CornerKickTakerPlayerId = request.CornerKickTakerPlayerId;
        matchTeamTactic.CaptainPlayerId = request.CaptainPlayerId;
        matchTeamTactic.ModifiedAt = now;
        matchTeamTactic.ModifiedBy = userId;
    }
}
