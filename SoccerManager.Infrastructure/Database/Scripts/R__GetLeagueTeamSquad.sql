-- The squad of one league team, behind GET /api/league-teams/{id}/players: each player with their current rating and main position.
-- The current rating (the dbo.PlayerRatings row with IsCurrent = 1, at most one by UX_PlayerRatings_PlayerId_Current) is a LEFT JOIN,
-- unlike in dbo.SearchPlayers, so a player with no current rating is still listed, with a NULL Rating.
-- A player's main position comes from dbo.GetPlayerMainPosition, as in dbo.SearchPlayers.
-- Ordered by line (goalkeepers, defence, midfield, attack), then by dbo.Positions.CatalogOrder within the line
-- (positions sharing an order by name), a player with no position last of all; then by rating, highest first, then name.
-- Every squad row is returned as stored: a player registered twice (which V1_0_0_37 allows under a race) is listed twice.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamSquad
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.Id,
           p.Name,
           cr.Rating,
           p.ImageUrl,
           mp.PositionId,
           pos.Name AS PositionName,
           pos.ShortName AS PositionShortName
    FROM dbo.LeagueTeamPlayers ltp
        INNER JOIN dbo.Players p ON p.Id = ltp.PlayerId
        LEFT JOIN dbo.PlayerRatings cr ON cr.PlayerId = p.Id AND cr.IsCurrent = 1
        OUTER APPLY dbo.GetPlayerMainPosition(p.Id) mp
        LEFT JOIN dbo.Positions pos ON pos.Id = mp.PositionId
    WHERE ltp.LeagueTeamId = @LeagueTeamId
    ORDER BY CASE WHEN pos.Id IS NULL THEN 1 ELSE 0 END,
             pos.Area DESC,
             pos.CatalogOrder,
             pos.Name,
             pos.Id,
             cr.Rating DESC,
             p.Name,
             p.Id,
             ltp.Id;
END
