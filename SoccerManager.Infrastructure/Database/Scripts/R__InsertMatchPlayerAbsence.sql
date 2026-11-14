CREATE OR ALTER PROCEDURE dbo.InsertMatchPlayerAbsence
    @MatchId      INT,
    @LeagueTeamId INT,
    @PlayerId     INT,
    @Reason       TINYINT,
    @CreatedAt    DATETIME2(7),
    @ModifiedAt   DATETIME2(7),
    @CreatedBy    UNIQUEIDENTIFIER,
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- The club must play the match: dbo.MatchTeamStatistics is the only record of a match's two clubs, and
    -- dbo.InsertMatchTeamStatistic already keeps its club in the match's league.
    IF NOT EXISTS (SELECT 1 FROM dbo.MatchTeamStatistics WHERE MatchId = @MatchId AND LeagueTeamId = @LeagueTeamId)
        THROW 50000, 'The club does not play in this match.', 1;

    INSERT INTO dbo.MatchPlayerAbsences (MatchId, LeagueTeamId, PlayerId, Reason, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@MatchId, @LeagueTeamId, @PlayerId, @Reason, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
