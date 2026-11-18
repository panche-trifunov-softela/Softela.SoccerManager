-- @Commentary is NVARCHAR(MAX), not JSON: it converts implicitly into the column's native json type on insert.
CREATE OR ALTER PROCEDURE dbo.InsertMatch
    @SeasonId      INT,
    @CompetitionId INT,
    @RefereeId     INT,
    @StartDateTime DATETIME2(7),
    @Commentary    NVARCHAR(MAX),
    @Attendance    INT,
    @IsStarted     BIT,
    @CreatedAt     DATETIME2(7),
    @ModifiedAt    DATETIME2(7),
    @CreatedBy     UNIQUEIDENTIFIER,
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- A national competition's matches carry a World Cup stage, which only dbo.InsertStagedMatch writes (V1_0_0_69).
    IF EXISTS (SELECT 1 FROM dbo.Competitions WHERE Id = @CompetitionId AND IsNational = 1)
        THROW 50000, 'A national competition''s matches are written with their World Cup stage.', 1;

    INSERT INTO dbo.Matches (SeasonId, CompetitionId, RefereeId, StartDateTime, Commentary, Attendance, IsStarted, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@SeasonId, @CompetitionId, @RefereeId, @StartDateTime, @Commentary, @Attendance, @IsStarted, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
