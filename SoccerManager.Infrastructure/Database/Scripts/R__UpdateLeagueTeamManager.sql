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

    -- A national team is on a League's roster to play its national competition (V1_0_0_68); a national team manager waits for national team management.
    IF EXISTS (SELECT 1 FROM dbo.LeagueTeams WHERE Id = @LeagueTeamId AND NationalTeamId IS NOT NULL)
        THROW 50000, 'A national team cannot have a manager yet.', 1;

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
