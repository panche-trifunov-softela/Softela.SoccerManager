-- A user's pending applications (Status 1) that can still be accepted, in every league, newest first (Id DESC breaks a tie),
-- with the league and team names and the team's logo for display.
-- The user is reached through dbo.Managers (applications carry only ManagerId), and the league and team through the roster
-- entry (dbo.LeagueTeams); every join is INNER over NOT NULL foreign keys, so a user with no manager profile yields no rows.
-- Two kinds are skipped because accepting them is refused: an application to a club with a current manager (the test in
-- dbo.GetPendingLeagueTeamManagerApplicationsByUserAndLeague), and one in a league where the applicant already manages a club.
-- Accepting an application rejects the other pending applications it makes unacceptable; a direct appointment does not, so
-- those stay pending.
-- UQ_Managers_UserId, IX_LeagueTeamManagerApplications_ManagerId, UX_LeagueTeamManagers_LeagueTeamId_Current and
-- IX_LeagueTeamManagers_ManagerId serve the lookups.
CREATE OR ALTER PROCEDURE dbo.GetPendingLeagueTeamManagerApplicationsByUserId
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ltma.Id,
           ltma.LeagueTeamId,
           lt.LeagueId,
           l.Name    AS LeagueName,
           t.Name    AS TeamName,
           t.LogoUrl AS TeamLogoUrl,
           ltma.CreatedAt
    FROM dbo.LeagueTeamManagerApplications ltma
        INNER JOIN dbo.Managers    m  ON m.Id = ltma.ManagerId
        INNER JOIN dbo.LeagueTeams lt ON lt.Id = ltma.LeagueTeamId
        INNER JOIN dbo.Leagues     l  ON l.Id = lt.LeagueId
        INNER JOIN dbo.Teams       t  ON t.Id = lt.TeamId
    WHERE m.UserId = @UserId
      AND ltma.Status = 1
      AND NOT EXISTS (
          SELECT 1
          FROM dbo.LeagueTeamManagers ltm
          WHERE ltm.LeagueTeamId = ltma.LeagueTeamId
            AND ltm.IsCurrent = 1
      )
      AND NOT EXISTS (
          SELECT 1
          FROM dbo.LeagueTeamManagers ltm2
              INNER JOIN dbo.LeagueTeams lt2 ON lt2.Id = ltm2.LeagueTeamId
          WHERE ltm2.ManagerId = ltma.ManagerId
            AND ltm2.IsCurrent = 1
            AND lt2.LeagueId = lt.LeagueId
      )
    ORDER BY ltma.CreatedAt DESC, ltma.Id DESC;
END
