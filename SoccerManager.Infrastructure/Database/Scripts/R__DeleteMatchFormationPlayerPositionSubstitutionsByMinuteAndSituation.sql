-- Clears one club's substitution lineup for a match at a minute and situation, in the same transaction as its tactic row. This
-- is a hard delete; the table has no soft-delete flag. The handler's access check, not this procedure, decides whether the
-- match may still change.
CREATE OR ALTER PROCEDURE dbo.DeleteMatchFormationPlayerPositionSubstitutionsByMinuteAndSituation
    @MatchId      INT,
    @LeagueTeamId INT,
    @MinuteAt     INT,
    @Situation    TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.MatchFormationPlayerPositionSubstitutions
    WHERE MatchId = @MatchId
      AND LeagueTeamId = @LeagueTeamId
      AND MinuteAt = @MinuteAt
      AND Situation = @Situation;
END
