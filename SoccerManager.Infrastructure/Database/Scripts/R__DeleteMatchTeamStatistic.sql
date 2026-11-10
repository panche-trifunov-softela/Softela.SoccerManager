CREATE OR ALTER PROCEDURE dbo.DeleteMatchTeamStatistic
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- A club's player statistics and shootout kicks count for it only through this row, so deleting the row
    -- would drop them out of the match's score.
    IF EXISTS (
        SELECT 1
        FROM dbo.MatchTeamStatistics mts
        WHERE mts.Id = @Id
          AND (EXISTS (SELECT 1 FROM dbo.MatchPlayerStatistics p WITH (UPDLOCK, HOLDLOCK) WHERE p.MatchId = mts.MatchId AND p.LeagueTeamId = mts.LeagueTeamId)
               OR EXISTS (SELECT 1 FROM dbo.PenaltiesAfterExtraTime k WITH (UPDLOCK, HOLDLOCK) WHERE k.MatchId = mts.MatchId AND k.LeagueTeamId = mts.LeagueTeamId)))
        THROW 50000, 'The club has player statistics or shootout kicks in this match.', 1;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.MatchTeamStatistics WHERE Id = @Id;
END
