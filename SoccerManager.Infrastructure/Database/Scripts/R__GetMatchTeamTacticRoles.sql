-- The four roles of one club's main tactic for a match, which a new substitution starts from; no row when the club has not
-- saved instructions for the match yet. It never writes.
CREATE OR ALTER PROCEDURE dbo.GetMatchTeamTacticRoles
    @MatchId      INT,
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT PenaltyTakerPlayerId,
           FreeKickTakerPlayerId,
           CornerKickTakerPlayerId,
           CaptainPlayerId
    FROM dbo.MatchTeamTactics
    WHERE MatchId = @MatchId
      AND LeagueTeamId = @LeagueTeamId;
END
