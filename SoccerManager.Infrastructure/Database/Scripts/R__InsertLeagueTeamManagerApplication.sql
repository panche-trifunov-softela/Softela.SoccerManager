CREATE OR ALTER PROCEDURE dbo.InsertLeagueTeamManagerApplication
    @LeagueTeamId INT,
    @ManagerId    INT,
    @Status       TINYINT,
    @ResponseDate DATETIME2(7),
    @CreatedAt    DATETIME2(7),
    @ModifiedAt   DATETIME2(7),
    @CreatedBy    UNIQUEIDENTIFIER,
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.LeagueTeamManagerApplications (LeagueTeamId, ManagerId, Status, ResponseDate, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueTeamId, @ManagerId, @Status, @ResponseDate, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
