CREATE OR ALTER PROCEDURE dbo.UpdateLeagueTeamManager
    @Id           INT,
    @LeagueTeamId INT,
    @ManagerId    INT,
    @StartDate    DATE,
    @EndDate      DATE,
    @IsCurrent    BIT,
    @ModifiedAt   DATETIME2(7),
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.LeagueTeamManagers
    SET LeagueTeamId = @LeagueTeamId,
        ManagerId    = @ManagerId,
        StartDate    = @StartDate,
        EndDate      = @EndDate,
        IsCurrent    = @IsCurrent,
        ModifiedAt   = @ModifiedAt,
        ModifiedBy   = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
