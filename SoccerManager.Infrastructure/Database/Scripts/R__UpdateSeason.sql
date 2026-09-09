CREATE OR ALTER PROCEDURE dbo.UpdateSeason
    @Id           INT,
    @SeasonNumber INT,
    @ModifiedAt   DATETIME2(7),
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- LeagueId is fixed at creation, so a season never moves between leagues.
    UPDATE dbo.Seasons
    SET SeasonNumber = @SeasonNumber,
        ModifiedAt   = @ModifiedAt,
        ModifiedBy   = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
