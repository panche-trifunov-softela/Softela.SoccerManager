CREATE OR ALTER PROCEDURE dbo.InsertNationalTeamMatch
    @SeasonId           INT,
    @HomeNationalTeamId INT,
    @AwayNationalTeamId INT,
    @StartDateTime      DATETIME2(7),
    @Stage              TINYINT,
    @GroupNumber        TINYINT,
    @RoundNumber        TINYINT,
    @Leg                TINYINT,
    @IsStarted          BIT,
    @CreatedAt          DATETIME2(7),
    @ModifiedAt         DATETIME2(7),
    @CreatedBy          UNIQUEIDENTIFIER,
    @ModifiedBy         UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- IsFinished is left to DF_NationalTeamMatches_IsFinished, so a new match is unfinished, as dbo.InsertMatch leaves dbo.Matches.IsFinished to its default.
    -- The API calls this, and the Generator is to write the World Cup's matches through it as well, so its parameters are a contract with the Generator too.
    INSERT INTO dbo.NationalTeamMatches (SeasonId, HomeNationalTeamId, AwayNationalTeamId, StartDateTime, Stage, GroupNumber, RoundNumber, Leg, IsStarted, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@SeasonId, @HomeNationalTeamId, @AwayNationalTeamId, @StartDateTime, @Stage, @GroupNumber, @RoundNumber, @Leg, @IsStarted, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
