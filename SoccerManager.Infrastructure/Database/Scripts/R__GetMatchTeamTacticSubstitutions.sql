-- One club's substitutions for a match, ordered by minute, then situation: the order the API lists them in. It never writes.
CREATE OR ALTER PROCEDURE dbo.GetMatchTeamTacticSubstitutions
    @MatchId      INT,
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id,
           MatchId,
           LeagueTeamId,
           MinuteAt,
           Situation,
           Mentality,
           Tempo,
           Passing,
           Width,
           Pressing,
           Tackling,
           AttackingSide,
           PenaltyTakerPlayerId,
           FreeKickTakerPlayerId,
           CornerKickTakerPlayerId,
           CaptainPlayerId,
           CreatedAt,
           ModifiedAt,
           CreatedBy,
           ModifiedBy
    FROM dbo.MatchTeamTacticSubstitutions
    WHERE MatchId = @MatchId
      AND LeagueTeamId = @LeagueTeamId
    ORDER BY MinuteAt, Situation;
END
