CREATE OR ALTER PROCEDURE dbo.InsertDivision
    @LeagueId        INT,
    @Name            NVARCHAR(100),
    @Order           INT,
    @TeamsPromoted   INT,
    @TeamsRelegated  INT,
    @TeamsInPlayoffs INT,
    @CreatedAt       DATETIME2(7),
    @ModifiedAt      DATETIME2(7),
    @CreatedBy       UNIQUEIDENTIFIER,
    @ModifiedBy      UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Divisions (LeagueId, Name, [Order], TeamsPromoted, TeamsRelegated, TeamsInPlayoffs, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueId, @Name, @Order, @TeamsPromoted, @TeamsRelegated, @TeamsInPlayoffs, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
