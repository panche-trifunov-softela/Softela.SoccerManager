-- The minute of one club's substitutions for a match, or NULL when it has none; a club's substitutions in one match share one
-- minute. UPDLOCK and HOLDLOCK keep the club's key range locked until the caller's transaction ends (as dbo.EnsureManager does),
-- so two saves that would each create a club's first substitution at different minutes run one after the other: the second
-- waits, then reads the first one's minute.
CREATE OR ALTER PROCEDURE dbo.GetMatchTeamTacticSubstitutionMinute
    @MatchId      INT,
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MIN(MinuteAt) AS MinuteAt
    FROM dbo.MatchTeamTacticSubstitutions WITH (UPDLOCK, HOLDLOCK)
    WHERE MatchId = @MatchId
      AND LeagueTeamId = @LeagueTeamId;
END
