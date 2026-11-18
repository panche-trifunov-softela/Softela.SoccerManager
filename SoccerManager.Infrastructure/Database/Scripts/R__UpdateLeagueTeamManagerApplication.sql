CREATE OR ALTER PROCEDURE dbo.UpdateLeagueTeamManagerApplication
    @Id           INT,
    @Status       TINYINT,
    @ResponseDate DATETIME2(7),
    @ModifiedAt   DATETIME2(7),
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- A national team is on a League's roster to play its national competition (V1_0_0_68); applying for a national team waits for national team management.
    -- This proc has no @LeagueTeamId, so it guards the roster row the application is stored against.
    IF EXISTS (
        SELECT 1
        FROM dbo.LeagueTeamManagerApplications a
            INNER JOIN dbo.LeagueTeams lt ON lt.Id = a.LeagueTeamId
        WHERE a.Id = @Id AND lt.NationalTeamId IS NOT NULL)
        THROW 50000, 'A national team cannot be applied for yet.', 1;

    -- Only the answer changes; LeagueTeamId and ManagerId are fixed once the application exists.
    UPDATE dbo.LeagueTeamManagerApplications
    SET Status       = @Status,
        ResponseDate = @ResponseDate,
        ModifiedAt   = @ModifiedAt,
        ModifiedBy   = @ModifiedBy
    WHERE Id = @Id AND Status = 1;

    -- Only a pending application can be answered; 0 means someone else answered it in between.
    SELECT CASE WHEN @@ROWCOUNT = 0 THEN 0 ELSE @Id END;
END
