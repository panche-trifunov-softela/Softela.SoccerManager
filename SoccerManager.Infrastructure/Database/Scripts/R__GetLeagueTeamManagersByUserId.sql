-- The join through dbo.Managers is how a Keycloak user id reaches an appointment: LeagueTeamManagers carries only ManagerId, never UserId.
-- The league and team come through the appointment's roster entry (dbo.LeagueTeams). Every join is INNER because LeagueTeamManagers.ManagerId and LeagueTeamId, and LeagueTeams.LeagueId and TeamId, are all NOT NULL foreign keys, so a user with no manager profile simply yields no rows rather than an error.
-- LeagueId is returned for the application validators ("already manages a club in this league"); the API response carries only LeagueTeamId.
-- Ordered by IsCurrent DESC, StartDate DESC so current appointments come back first, then the most recently started.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamManagersByUserId
    @UserId      UNIQUEIDENTIFIER,
    @CurrentOnly BIT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ltm.Id,
           ltm.LeagueTeamId,
           lt.LeagueId,
           l.Name       AS LeagueName,
           t.Name       AS TeamName,
           t.LogoUrl    AS TeamLogoUrl,
           ltm.ManagerId,
           ltm.StartDate,
           ltm.EndDate,
           ltm.IsCurrent,
           ltm.CreatedAt,
           ltm.ModifiedAt
    FROM dbo.LeagueTeamManagers ltm
        INNER JOIN dbo.Managers    m  ON m.Id = ltm.ManagerId
        INNER JOIN dbo.LeagueTeams lt ON lt.Id = ltm.LeagueTeamId
        INNER JOIN dbo.Leagues     l  ON l.Id = lt.LeagueId
        INNER JOIN dbo.Teams       t  ON t.Id = lt.TeamId
    WHERE m.UserId = @UserId
      AND (@CurrentOnly = 0 OR ltm.IsCurrent = 1)
    ORDER BY ltm.IsCurrent DESC, ltm.StartDate DESC;
END
