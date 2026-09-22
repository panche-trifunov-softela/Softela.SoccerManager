-- The join through dbo.Managers is how a Keycloak user id reaches an appointment: LeagueTeamManagers carries only ManagerId, never UserId.
-- Every join is INNER because LeagueTeamManagers.ManagerId, LeagueId and TeamId are all NOT NULL foreign keys, so a user with no manager profile simply yields no rows rather than an error.
-- Ordered by IsCurrent DESC, StartDate DESC so current appointments come back first, then the most recently started.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamManagersByUserId
    @UserId      UNIQUEIDENTIFIER,
    @CurrentOnly BIT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ltm.Id,
           ltm.LeagueId,
           l.Name       AS LeagueName,
           ltm.TeamId,
           t.Name       AS TeamName,
           t.LogoUrl    AS TeamLogoUrl,
           ltm.ManagerId,
           ltm.StartDate,
           ltm.EndDate,
           ltm.IsCurrent,
           ltm.CreatedAt,
           ltm.ModifiedAt
    FROM dbo.LeagueTeamManagers ltm
        INNER JOIN dbo.Managers m ON m.Id = ltm.ManagerId
        INNER JOIN dbo.Leagues   l ON l.Id = ltm.LeagueId
        INNER JOIN dbo.Teams     t ON t.Id = ltm.TeamId
    WHERE m.UserId = @UserId
      AND (@CurrentOnly = 0 OR ltm.IsCurrent = 1)
    ORDER BY ltm.IsCurrent DESC, ltm.StartDate DESC;
END
