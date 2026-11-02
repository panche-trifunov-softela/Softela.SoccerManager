-- Clears one club's whole lineup for a match; the auto pick runs it before it writes the picked lineup. This is a hard delete; the
-- table has no soft-delete flag. The handler's access check, not this procedure, decides whether the match may still change.
CREATE OR ALTER PROCEDURE dbo.DeleteMatchFormationPlayerPositionsByMatchAndLeagueTeam
    @MatchId      INT,
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.MatchFormationPlayerPositions
    WHERE MatchId = @MatchId
      AND LeagueTeamId = @LeagueTeamId;
END
