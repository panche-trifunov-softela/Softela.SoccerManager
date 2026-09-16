CREATE OR ALTER PROCEDURE dbo.InsertLeagueTeamManager
    @LeagueId   INT,
    @TeamId     INT,
    @ManagerId  INT,
    @StartDate  DATE,
    @EndDate    DATE,
    @IsCurrent  BIT,
    @CreatedAt  DATETIME2(7),
    @ModifiedAt DATETIME2(7),
    @CreatedBy  UNIQUEIDENTIFIER,
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.LeagueTeamManagers (LeagueId, TeamId, ManagerId, StartDate, EndDate, IsCurrent, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueId, @TeamId, @ManagerId, @StartDate, @EndDate, @IsCurrent, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
