CREATE OR ALTER PROCEDURE dbo.UpdateMatchTeamStatistic
    @Id            INT,
    @LeagueTeamId  INT,
    @MatchId       INT,
    @IsHomeTeam    BIT,
    @ShotsTotal    INT,
    @ShotsOnTarget INT,
    @Possession    INT,
    @ModifiedAt    DATETIME2(7),
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- The same guard as dbo.InsertMatchTeamStatistic: an update can move the row to another match or club.
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.LeagueTeams lt
            INNER JOIN dbo.Seasons se ON se.LeagueId = lt.LeagueId
            INNER JOIN dbo.Matches m ON m.SeasonId = se.Id
        WHERE lt.Id = @LeagueTeamId AND m.Id = @MatchId)
        THROW 50000, 'The club and the match must belong to the same league.', 1;

    -- A national competition is played by national teams and every other competition by clubs; a friendly, which has no
    -- competition, by two clubs or by two national teams. A club never meets a national team.
    IF EXISTS (
        SELECT 1
        FROM dbo.Matches m
            INNER JOIN dbo.LeagueTeams lt ON lt.Id = @LeagueTeamId
            LEFT JOIN dbo.Competitions c ON c.Id = m.CompetitionId
        WHERE m.Id = @MatchId
          AND ((c.Id IS NOT NULL AND c.IsNational <> CASE WHEN lt.NationalTeamId IS NULL THEN 0 ELSE 1 END)
               OR EXISTS (
                   SELECT 1
                   FROM dbo.MatchTeamStatistics o
                       INNER JOIN dbo.LeagueTeams olt ON olt.Id = o.LeagueTeamId
                   WHERE o.MatchId = @MatchId
                     AND o.Id <> @Id
                     AND o.LeagueTeamId <> @LeagueTeamId
                     AND CASE WHEN olt.NationalTeamId IS NULL THEN 0 ELSE 1 END <> CASE WHEN lt.NationalTeamId IS NULL THEN 0 ELSE 1 END)))
        THROW 50000, 'A club and a national team cannot play each other, and a national competition is only for national teams.', 1;

    -- This row is the only record that the club plays the match. Moving it to another match or club would drop the
    -- club's player statistics and shootout kicks out of the match's score, and leave its absences on a club that no
    -- longer plays the match.
    IF EXISTS (
        SELECT 1
        FROM dbo.MatchTeamStatistics mts
        WHERE mts.Id = @Id
          AND (mts.MatchId <> @MatchId OR mts.LeagueTeamId <> @LeagueTeamId)
          AND (EXISTS (SELECT 1 FROM dbo.MatchPlayerStatistics p WITH (UPDLOCK, HOLDLOCK) WHERE p.MatchId = mts.MatchId AND p.LeagueTeamId = mts.LeagueTeamId)
               OR EXISTS (SELECT 1 FROM dbo.PenaltiesAfterExtraTime k WITH (UPDLOCK, HOLDLOCK) WHERE k.MatchId = mts.MatchId AND k.LeagueTeamId = mts.LeagueTeamId)
               OR EXISTS (SELECT 1 FROM dbo.MatchPlayerAbsences a WITH (UPDLOCK, HOLDLOCK) WHERE a.MatchId = mts.MatchId AND a.LeagueTeamId = mts.LeagueTeamId)))
        THROW 50000, 'The club has player statistics, shootout kicks or absences in this match.', 1;

    UPDATE dbo.MatchTeamStatistics
    SET LeagueTeamId  = @LeagueTeamId,
        MatchId       = @MatchId,
        IsHomeTeam    = @IsHomeTeam,
        ShotsTotal    = @ShotsTotal,
        ShotsOnTarget = @ShotsOnTarget,
        Possession    = @Possession,
        ModifiedAt    = @ModifiedAt,
        ModifiedBy    = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
