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
