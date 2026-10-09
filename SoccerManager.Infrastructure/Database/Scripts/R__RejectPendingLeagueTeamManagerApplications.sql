CREATE OR ALTER PROCEDURE dbo.RejectPendingLeagueTeamManagerApplications
    @AcceptedId   INT,
    @LeagueId     INT,
    @TeamId       INT,
    @ManagerId    INT,
    @ResponseDate DATETIME2(7),
    @ModifiedAt   DATETIME2(7),
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Rejects the pending applications an acceptance makes moot: the other ones for the same team in the league, and the applicant's other ones in the league.
    -- Status 1 is ApplicationStatus Pending and 3 is Rejected.
    UPDATE dbo.LeagueTeamManagerApplications
    SET Status = 3,
        ResponseDate = @ResponseDate,
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    WHERE Status = 1
      AND Id <> @AcceptedId
      AND LeagueId = @LeagueId
      AND (TeamId = @TeamId OR ManagerId = @ManagerId);
END
