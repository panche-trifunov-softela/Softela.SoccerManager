CREATE OR ALTER PROCEDURE dbo.DeleteMatchTeamStatistic
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This row is the only record that the club plays the match. Deleting it would drop the club's player statistics
    -- and shootout kicks out of the match's score, and leave its absences on a club that no longer plays the match.
    IF EXISTS (
        SELECT 1
        FROM dbo.MatchTeamStatistics mts
        WHERE mts.Id = @Id
          AND (EXISTS (SELECT 1 FROM dbo.MatchPlayerStatistics p WITH (UPDLOCK, HOLDLOCK) WHERE p.MatchId = mts.MatchId AND p.LeagueTeamId = mts.LeagueTeamId)
               OR EXISTS (SELECT 1 FROM dbo.PenaltiesAfterExtraTime k WITH (UPDLOCK, HOLDLOCK) WHERE k.MatchId = mts.MatchId AND k.LeagueTeamId = mts.LeagueTeamId)
               OR EXISTS (SELECT 1 FROM dbo.MatchPlayerAbsences a WITH (UPDLOCK, HOLDLOCK) WHERE a.MatchId = mts.MatchId AND a.LeagueTeamId = mts.LeagueTeamId)))
        THROW 50000, 'The club has player statistics, shootout kicks or absences in this match.', 1;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.MatchTeamStatistics WHERE Id = @Id;
END
