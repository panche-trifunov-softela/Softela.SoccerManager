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

    -- A national team is on a League's roster to play its national competition (V1_0_0_68); a national team manager waits for national team management.
    IF EXISTS (SELECT 1 FROM dbo.LeagueTeams WHERE Id = @LeagueTeamId AND NationalTeamId IS NOT NULL)
        THROW 50000, 'A national team cannot have a manager yet.', 1;

    INSERT INTO dbo.LeagueTeamManagers (LeagueTeamId, ManagerId, StartDate, EndDate, IsCurrent, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueTeamId, @ManagerId, @StartDate, @EndDate, @IsCurrent, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
