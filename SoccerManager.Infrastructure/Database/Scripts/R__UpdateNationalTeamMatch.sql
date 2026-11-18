CREATE OR ALTER PROCEDURE dbo.UpdateNationalTeamMatch
    @Id                 INT,
    @SeasonId           INT,
    @HomeNationalTeamId INT,
    @AwayNationalTeamId INT,
    @StartDateTime      DATETIME2(7),
    @Stage              TINYINT,
    @GroupNumber        TINYINT,
    @RoundNumber        TINYINT,
    @Leg                TINYINT,
    @IsStarted          BIT,
    @IsFinished         BIT,
    @ModifiedAt         DATETIME2(7),
    @ModifiedBy         UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.NationalTeamMatches
    SET SeasonId           = @SeasonId,
        HomeNationalTeamId = @HomeNationalTeamId,
        AwayNationalTeamId = @AwayNationalTeamId,
        StartDateTime      = @StartDateTime,
        Stage              = @Stage,
        GroupNumber        = @GroupNumber,
        RoundNumber        = @RoundNumber,
        Leg                = @Leg,
        IsStarted          = @IsStarted,
        IsFinished         = @IsFinished,
        ModifiedAt         = @ModifiedAt,
        ModifiedBy         = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
