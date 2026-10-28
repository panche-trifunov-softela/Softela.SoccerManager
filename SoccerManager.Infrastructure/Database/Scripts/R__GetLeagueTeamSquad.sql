-- The squad of one league team, behind GET /api/league-teams/{id}/players: each player with their current rating and main position.
-- The current rating (the dbo.PlayerRatings row with IsCurrent = 1, at most one by UX_PlayerRatings_PlayerId_Current) is a LEFT JOIN,
-- unlike in dbo.SearchPlayers, so a player with no current rating is still listed, with a NULL Rating.
-- A player's main position comes from dbo.GetPlayerMainPosition, as in dbo.SearchPlayers.
-- Ordered by line (goalkeepers, defence, midfield, attack), then by the importer's catalog order within the line
-- (SoccerManager.Importer/Transform/PositionCatalog.cs, kept in step with the CASE below), a position outside the catalog last in its
-- line and a player with no position last of all; then by rating, highest first, then name.
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
           pos.Name AS PositionName
    FROM dbo.LeagueTeamPlayers ltp
        INNER JOIN dbo.Players p ON p.Id = ltp.PlayerId
        LEFT JOIN dbo.PlayerRatings cr ON cr.PlayerId = p.Id AND cr.IsCurrent = 1
        OUTER APPLY dbo.GetPlayerMainPosition(p.Id) mp
        LEFT JOIN dbo.Positions pos ON pos.Id = mp.PositionId
    WHERE ltp.LeagueTeamId = @LeagueTeamId
    ORDER BY CASE WHEN pos.Id IS NULL THEN 1 ELSE 0 END,
             pos.Area DESC,
             CASE pos.Name
                 WHEN 'Goalkeeper' THEN 1
                 WHEN 'Centre-Back' THEN 2
                 WHEN 'Left-Back' THEN 3
                 WHEN 'Right-Back' THEN 4
                 WHEN 'Defensive Midfield' THEN 5
                 WHEN 'Central Midfield' THEN 6
                 WHEN 'Attacking Midfield' THEN 7
                 WHEN 'Left Midfield' THEN 8
                 WHEN 'Right Midfield' THEN 9
                 WHEN 'Left Winger' THEN 10
                 WHEN 'Right Winger' THEN 11
                 WHEN 'Second Striker' THEN 12
                 WHEN 'Centre-Forward' THEN 13
                 ELSE 14
             END,
             pos.Name,
             pos.Id,
             cr.Rating DESC,
             p.Name,
             p.Id,
             ltp.Id;
END
