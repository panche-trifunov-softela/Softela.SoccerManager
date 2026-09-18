CREATE OR ALTER PROCEDURE dbo.InsertMatchTeamStatistic
    @TeamId        INT,
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

    INSERT INTO dbo.MatchTeamStatistics (TeamId, MatchId, IsHomeTeam, ShotsTotal, ShotsOnTarget, Possession, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@TeamId, @MatchId, @IsHomeTeam, @ShotsTotal, @ShotsOnTarget, @Possession, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
