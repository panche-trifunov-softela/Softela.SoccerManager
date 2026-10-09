CREATE OR ALTER PROCEDURE dbo.UpdateLeagueTeamManagerApplication
    @Id           INT,
    @Status       TINYINT,
    @ResponseDate DATETIME2(7),
    @ModifiedAt   DATETIME2(7),
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Only the answer changes; LeagueId, TeamId and ManagerId are fixed once the application exists.
    UPDATE dbo.LeagueTeamManagerApplications
    SET Status       = @Status,
        ResponseDate = @ResponseDate,
        ModifiedAt   = @ModifiedAt,
        ModifiedBy   = @ModifiedBy
    WHERE Id = @Id AND Status = 1;

    -- Only a pending application can be answered; 0 means someone else answered it in between.
    SELECT CASE WHEN @@ROWCOUNT = 0 THEN 0 ELSE @Id END;
END
