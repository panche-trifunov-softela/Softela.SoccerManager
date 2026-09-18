-- @Commentary is NVARCHAR(MAX), not JSON: it converts implicitly into the column's native json type on insert.
CREATE OR ALTER PROCEDURE dbo.InsertMatch
    @SeasonId      INT,
    @DivisionId    INT,
    @RefereeId     INT,
    @StartDateTime DATETIME2(7),
    @Commentary    NVARCHAR(MAX),
    @Attendance    INT,
    @CreatedAt     DATETIME2(7),
    @ModifiedAt    DATETIME2(7),
    @CreatedBy     UNIQUEIDENTIFIER,
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Matches (SeasonId, DivisionId, RefereeId, StartDateTime, Commentary, Attendance, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@SeasonId, @DivisionId, @RefereeId, @StartDateTime, @Commentary, @Attendance, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
