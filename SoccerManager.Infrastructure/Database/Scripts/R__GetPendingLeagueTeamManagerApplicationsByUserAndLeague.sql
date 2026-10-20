-- A user's pending applications (Status 1) for the clubs of one league, newest first (Id DESC breaks a tie).
-- The user is reached through dbo.Managers (applications carry only ManagerId) and the league through the roster entry
-- (dbo.LeagueTeams); both joins are INNER over NOT NULL foreign keys, so a user with no manager profile yields no rows.
-- A club with a current manager is skipped, by the same test as IsManaged in dbo.GetAvailableLeagueTeams: its application can
-- never be accepted, so it must not stop the applicant from applying to another club of the league.
-- UQ_Managers_UserId, IX_LeagueTeamManagerApplications_ManagerId and UX_LeagueTeamManagers_LeagueTeamId_Current serve the lookups.
CREATE OR ALTER PROCEDURE dbo.GetPendingLeagueTeamManagerApplicationsByUserAndLeague
    @UserId   UNIQUEIDENTIFIER,
    @LeagueId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ltma.Id,
           ltma.LeagueTeamId,
           ltma.ManagerId,
           ltma.Status,
           ltma.ResponseDate,
           ltma.CreatedAt,
           ltma.ModifiedAt,
           ltma.CreatedBy,
           ltma.ModifiedBy
    FROM dbo.LeagueTeamManagerApplications ltma
        INNER JOIN dbo.Managers    m  ON m.Id = ltma.ManagerId
        INNER JOIN dbo.LeagueTeams lt ON lt.Id = ltma.LeagueTeamId
    WHERE m.UserId = @UserId
      AND lt.LeagueId = @LeagueId
      AND ltma.Status = 1
      AND NOT EXISTS (
          SELECT 1
          FROM dbo.LeagueTeamManagers ltm
          WHERE ltm.LeagueTeamId = ltma.LeagueTeamId
            AND ltm.IsCurrent = 1
      )
    ORDER BY ltma.CreatedAt DESC, ltma.Id DESC;
END
