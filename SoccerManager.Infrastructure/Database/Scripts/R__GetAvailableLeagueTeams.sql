-- Every club on a league's roster, by team name (the roster Id breaks ties), with IsManaged set when the club has a current manager there.
-- A club whose appointments have all ended is not managed, and pending applications do not count: answering one re-checks the club.
-- UQ_LeagueTeams_LeagueId_TeamId serves the roster lookup and UX_LeagueTeamManagers_LeagueTeamId_Current the current-manager check.
CREATE OR ALTER PROCEDURE dbo.GetAvailableLeagueTeams
    @LeagueId INT
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
           ) THEN 1 ELSE 0 END AS BIT) AS IsManaged
    FROM dbo.LeagueTeams lt
        INNER JOIN dbo.Teams t ON t.Id = lt.TeamId
    WHERE lt.LeagueId = @LeagueId
    ORDER BY t.Name, lt.Id;
END
