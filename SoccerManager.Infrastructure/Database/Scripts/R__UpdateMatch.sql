-- @Commentary is NVARCHAR(MAX), not JSON: it converts implicitly into the column's native json type on update.
CREATE OR ALTER PROCEDURE dbo.UpdateMatch
    @Id            INT,
    @SeasonId      INT,
    @CompetitionId INT,
    @RefereeId     INT,
    @StartDateTime DATETIME2(7),
    @Commentary    NVARCHAR(MAX),
    @Attendance    INT,
    @IsStarted     BIT,
    @ModifiedAt    DATETIME2(7),
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Matches
    SET SeasonId      = @SeasonId,
        CompetitionId = @CompetitionId,
        RefereeId     = @RefereeId,
        StartDateTime = @StartDateTime,
        Commentary    = @Commentary,
        Attendance    = @Attendance,
        IsStarted     = @IsStarted,
        ModifiedAt    = @ModifiedAt,
        ModifiedBy    = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
