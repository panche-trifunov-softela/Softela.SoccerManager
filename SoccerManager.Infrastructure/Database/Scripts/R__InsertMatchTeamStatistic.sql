CREATE OR ALTER PROCEDURE dbo.InsertMatchTeamStatistic
    @LeagueTeamId  INT,
    @MatchId       INT,
    @IsHomeTeam    BIT,
    @ShotsTotal    INT,
    @ShotsOnTarget INT,
    @Possession    INT,
    @CreatedAt     DATETIME2(7),
    @ModifiedAt    DATETIME2(7),
    @CreatedBy     UNIQUEIDENTIFIER,
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- The club must be on the roster of the match's league (its season's league). A row for another
    -- league's club would block that league's delete, since FK_MatchTeamStatistics_LeagueTeams
    -- cannot cascade.
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
                     AND o.LeagueTeamId <> @LeagueTeamId
                     AND CASE WHEN olt.NationalTeamId IS NULL THEN 0 ELSE 1 END <> CASE WHEN lt.NationalTeamId IS NULL THEN 0 ELSE 1 END)))
        THROW 50000, 'A club and a national team cannot play each other, and a national competition is only for national teams.', 1;

    INSERT INTO dbo.MatchTeamStatistics (LeagueTeamId, MatchId, IsHomeTeam, ShotsTotal, ShotsOnTarget, Possession, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueTeamId, @MatchId, @IsHomeTeam, @ShotsTotal, @ShotsOnTarget, @Possession, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
