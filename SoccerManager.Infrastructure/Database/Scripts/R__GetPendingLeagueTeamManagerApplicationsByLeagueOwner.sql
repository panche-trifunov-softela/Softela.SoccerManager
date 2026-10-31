-- Pending applications (Status 1) that can still be accepted, for clubs in the leagues a user owns, or in every league when
-- @IsOperator = 1, newest first (Id DESC breaks a tie), with the league and team names, the team's logo and the applicant's
-- photo for display. @IsOperator opens every league, so it must come from the caller's token, never from a request value.
-- A user with no manager profile owns no league, so they get no rows unless they are an operator, and a league with no owner
-- (made before V1_0_0_47, or whose owner was deleted) is listed for operators only.
-- The user's own applications are skipped: dbo.GetPendingLeagueTeamManagerApplicationsByUserId lists them. So are the two
-- kinds that procedure skips because accepting them is refused: an application to a club with a current manager, and one in
-- a league where the applicant already manages a club.
-- UQ_Managers_UserId, IX_Leagues_OwnerManagerId, UQ_LeagueTeams_LeagueId_TeamId,
-- IX_LeagueTeamManagerApplications_LeagueTeamId, UX_LeagueTeamManagers_LeagueTeamId_Current and
-- IX_LeagueTeamManagers_ManagerId serve the lookups.
CREATE OR ALTER PROCEDURE dbo.GetPendingLeagueTeamManagerApplicationsByLeagueOwner
    @UserId     UNIQUEIDENTIFIER,
    @IsOperator BIT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ManagerId INT = (SELECT Id FROM dbo.Managers WHERE UserId = @UserId);

    SELECT ltma.Id,
           ltma.LeagueTeamId,
           lt.LeagueId,
           l.Name     AS LeagueName,
           t.Name     AS TeamName,
           t.LogoUrl  AS TeamLogoUrl,
           ltma.ManagerId,
           m.ImageUrl AS ManagerImageUrl,
           ltma.CreatedAt
    FROM dbo.LeagueTeamManagerApplications ltma
        INNER JOIN dbo.Managers    m  ON m.Id = ltma.ManagerId
        INNER JOIN dbo.LeagueTeams lt ON lt.Id = ltma.LeagueTeamId
        INNER JOIN dbo.Leagues     l  ON l.Id = lt.LeagueId
        INNER JOIN dbo.Teams       t  ON t.Id = lt.TeamId
    WHERE ltma.Status = 1
      AND (@IsOperator = 1 OR l.OwnerManagerId = @ManagerId)
      AND (@ManagerId IS NULL OR ltma.ManagerId <> @ManagerId)
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
