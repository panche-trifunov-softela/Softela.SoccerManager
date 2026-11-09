-- Removes one club's substitution tactic for a match at a minute and situation; its lineup rows are removed separately, in
-- the same transaction. This is a hard delete; the table has no soft-delete flag. No matching row is not an error.
CREATE OR ALTER PROCEDURE dbo.DeleteMatchTeamTacticSubstitution
    @MatchId      INT,
    @LeagueTeamId INT,
    @MinuteAt     INT,
    @Situation    TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.MatchTeamTacticSubstitutions
    WHERE MatchId = @MatchId
      AND LeagueTeamId = @LeagueTeamId
      AND MinuteAt = @MinuteAt
      AND Situation = @Situation;
END
