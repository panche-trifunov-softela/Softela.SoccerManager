-- Every club on a league's roster, by team name (the roster Id breaks ties), with IsManaged set when the club has a current manager there.
-- A club whose appointments have all ended is not managed, and pending applications do not count: answering one re-checks the club.
-- HasPendingApplication is set when the caller (@UserId, through their manager profile) has a pending application (Status 1) for the
-- club; another manager's does not count. The NULL default keeps a caller that passes no @UserId (an older API build on the shared
-- database) working, with the flag 0 for every club.
-- UQ_LeagueTeams_LeagueId_TeamId serves the roster lookup, UX_LeagueTeamManagers_LeagueTeamId_Current the current-manager check,
-- and UX_LeagueTeamManagerApplications_LeagueTeamId_ManagerId_Pending covers the pending-application check.
CREATE OR ALTER PROCEDURE dbo.GetAvailableLeagueTeams
    @LeagueId INT,
    @UserId   UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT lt.Id,
           t.Name           AS TeamName,
           t.LogoUrl        AS TeamLogoUrl,
           t.FinancialState AS TeamFinancialState,
           CAST(CASE WHEN EXISTS (
               SELECT 1
               FROM dbo.LeagueTeamManagers ltm
               WHERE ltm.LeagueTeamId = lt.Id
                 AND ltm.IsCurrent = 1
           ) THEN 1 ELSE 0 END AS BIT) AS IsManaged,
           CAST(CASE WHEN EXISTS (
               SELECT 1
               FROM dbo.LeagueTeamManagerApplications ltma
                   INNER JOIN dbo.Managers m ON m.Id = ltma.ManagerId
               WHERE ltma.LeagueTeamId = lt.Id
                 AND ltma.Status = 1
                 AND m.UserId = @UserId
           ) THEN 1 ELSE 0 END AS BIT) AS HasPendingApplication
    FROM dbo.LeagueTeams lt
        INNER JOIN dbo.Teams t ON t.Id = lt.TeamId
    WHERE lt.LeagueId = @LeagueId
    ORDER BY t.Name, lt.Id;
END
