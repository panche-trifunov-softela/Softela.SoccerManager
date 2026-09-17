-- @Commentary is NVARCHAR(MAX), not JSON: it converts implicitly into the column's native json type on update.
CREATE OR ALTER PROCEDURE dbo.UpdateMatch
    @Id            INT,
    @SeasonId      INT,
    @DivisionId    INT,
    @HomeTeamId    INT,
    @AwayTeamId    INT,
    @RefereeId     INT,
    @StartDateTime DATETIME2(7),
    @Commentary    NVARCHAR(MAX),
    @ModifiedAt    DATETIME2(7),
    @ModifiedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Matches
    SET SeasonId      = @SeasonId,
        DivisionId    = @DivisionId,
        HomeTeamId    = @HomeTeamId,
        AwayTeamId    = @AwayTeamId,
        RefereeId     = @RefereeId,
        StartDateTime = @StartDateTime,
        Commentary    = @Commentary,
        ModifiedAt    = @ModifiedAt,
        ModifiedBy    = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
