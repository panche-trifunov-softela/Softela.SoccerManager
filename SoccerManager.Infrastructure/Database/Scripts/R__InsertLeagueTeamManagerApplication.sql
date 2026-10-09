CREATE OR ALTER PROCEDURE dbo.InsertLeagueTeamManagerApplication
    @LeagueId     INT,
    @TeamId       INT,
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

    INSERT INTO dbo.LeagueTeamManagerApplications (LeagueId, TeamId, ManagerId, Status, ResponseDate, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueId, @TeamId, @ManagerId, @Status, @ResponseDate, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
