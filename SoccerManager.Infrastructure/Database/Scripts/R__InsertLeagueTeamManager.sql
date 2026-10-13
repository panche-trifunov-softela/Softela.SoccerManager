CREATE OR ALTER PROCEDURE dbo.InsertLeagueTeamManager
    @LeagueTeamId INT,
    @ManagerId    INT,
    @StartDate    DATE,
    @EndDate      DATE,
    @IsCurrent    BIT,
    @CreatedAt    DATETIME2(7),
    @ModifiedAt   DATETIME2(7),
    @CreatedBy    UNIQUEIDENTIFIER,
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.LeagueTeamManagers (LeagueTeamId, ManagerId, StartDate, EndDate, IsCurrent, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueTeamId, @ManagerId, @StartDate, @EndDate, @IsCurrent, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
