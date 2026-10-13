CREATE OR ALTER PROCEDURE dbo.RejectPendingLeagueTeamManagerApplications
    @AcceptedId   INT,
    @LeagueTeamId INT,
    @ManagerId    INT,
    @ResponseDate DATETIME2(7),
    @ModifiedAt   DATETIME2(7),
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- The accepted club's league, reached through its roster entry.
    DECLARE @LeagueId INT = (SELECT LeagueId FROM dbo.LeagueTeams WHERE Id = @LeagueTeamId);

    -- Rejects the pending applications an acceptance makes moot: the other ones for the same club, and the applicant's other ones in the club's league.
    -- Status 1 is ApplicationStatus Pending and 3 is Rejected.
    UPDATE dbo.LeagueTeamManagerApplications
    SET Status = 3,
        ResponseDate = @ResponseDate,
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    WHERE Status = 1
      AND Id <> @AcceptedId
      AND LeagueTeamId IN (SELECT Id FROM dbo.LeagueTeams WHERE LeagueId = @LeagueId)
      AND (LeagueTeamId = @LeagueTeamId OR ManagerId = @ManagerId);
END
