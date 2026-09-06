CREATE OR ALTER PROCEDURE dbo.InsertSeason
    @LeagueId     INT,
    @SeasonNumber INT,
    @CreatedAt    DATETIME2(7),
    @ModifiedAt   DATETIME2(7),
    @CreatedBy    UNIQUEIDENTIFIER,
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Seasons (LeagueId, SeasonNumber, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueId, @SeasonNumber, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
