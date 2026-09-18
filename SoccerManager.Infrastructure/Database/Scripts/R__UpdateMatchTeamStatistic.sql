CREATE OR ALTER PROCEDURE dbo.UpdateMatchTeamStatistic
    @Id            INT,
    @TeamId        INT,
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

    UPDATE dbo.MatchTeamStatistics
    SET TeamId        = @TeamId,
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
