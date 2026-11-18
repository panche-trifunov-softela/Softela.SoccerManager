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

    -- A national team is on a League's roster to play its national competition (V1_0_0_68); applying for a national team waits for national team management.
    IF EXISTS (SELECT 1 FROM dbo.LeagueTeams WHERE Id = @LeagueTeamId AND NationalTeamId IS NOT NULL)
        THROW 50000, 'A national team cannot be applied for yet.', 1;

    INSERT INTO dbo.LeagueTeamManagerApplications (LeagueTeamId, ManagerId, Status, ResponseDate, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueTeamId, @ManagerId, @Status, @ResponseDate, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
