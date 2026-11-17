-- One club's identity, behind GET /api/league-teams/{id}/profile: the league team, its league, and the team's name and logo,
-- the same for every caller. No row is returned for an unknown league team. It does not check access: the profile is public to
-- every signed-in user, as the league and team names already are.
-- The league team is looked up by its primary key; the team and the league are INNER JOINs because LeagueTeams.LeagueId and
-- TeamId are NOT NULL foreign keys.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamProfile
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT lt.Id       AS LeagueTeamId,
           lt.LeagueId,
           l.Name      AS LeagueName,
           t.Name      AS TeamName,
           t.LogoUrl   AS TeamLogoUrl
    FROM dbo.LeagueTeams lt
        INNER JOIN dbo.Teams   t ON t.Id = lt.TeamId
        INNER JOIN dbo.Leagues l ON l.Id = lt.LeagueId
    WHERE lt.Id = @LeagueTeamId;
END
