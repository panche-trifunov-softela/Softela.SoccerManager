-- One player's profile, behind GET /api/players/{id}/profile, as two result sets: the player's header row, then the player's positions.
-- The header is the player as the Players catalogue holds them: the real-life club and national team, the main position and the
-- current rating, with the player's value and wage. The club, national team and rating are LEFT JOINs, so a player with none of
-- them still gets a profile, with NULLs; unlike dbo.GetPlayerById, a player with no current rating is not left out. The main
-- position comes from dbo.GetPlayerMainPosition, so a player with no position gets NULL position columns, which the API returns
-- as position id 0 with empty names. No row at all means no player has that Id.
-- The positions are every dbo.PlayerPositions row of the player, best quality first, the main position therefore first. Each
-- carries the spot it is drawn at on a pitch: the average PitchX and PitchY of the starter slots in dbo.FormationPositions that
-- use that position, rounded to a whole percentage. Only starter slots have coordinates (V1_0_0_50), and a position no formation
-- uses gets NULL and is not drawn. AVG over a TINYINT returns an INT and truncates, hence the widening to DECIMAL first. The
-- aggregate stays inside the OUTER APPLY and names no outer column: SQL Server rejects an aggregate over an outer reference
-- (Msg 8124).
CREATE OR ALTER PROCEDURE dbo.GetPlayerProfile
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.Id,
           p.Name,
           p.ImageUrl,
           p.DateOfBirth,
           p.TeamId,
           t.Name AS TeamName,
           t.LogoUrl AS TeamLogoUrl,
           p.NationalTeamId,
           nt.Name AS NationalTeamName,
           nt.LogoUrl AS NationalTeamLogoUrl,
           mp.PositionId,
           pos.Name AS PositionName,
           pos.ShortName AS PositionShortName,
           cr.Rating,
           p.[Value],
           p.Wage
    FROM dbo.Players p
        LEFT JOIN dbo.Teams t ON t.Id = p.TeamId
        LEFT JOIN dbo.NationalTeams nt ON nt.Id = p.NationalTeamId
        LEFT JOIN dbo.PlayerRatings cr ON cr.PlayerId = p.Id AND cr.IsCurrent = 1
        OUTER APPLY dbo.GetPlayerMainPosition(p.Id) mp
        LEFT JOIN dbo.Positions pos ON pos.Id = mp.PositionId
    WHERE p.Id = @PlayerId;

    SELECT pp.PositionId,
           pos.Name,
           pos.ShortName,
           CAST(CASE WHEN pp.PositionId = mp.PositionId THEN 1 ELSE 0 END AS BIT) AS IsMain,
           spot.PitchX,
           spot.PitchY
    FROM dbo.PlayerPositions pp
        INNER JOIN dbo.Positions pos ON pos.Id = pp.PositionId
        OUTER APPLY dbo.GetPlayerMainPosition(pp.PlayerId) mp
        OUTER APPLY (
            SELECT CAST(ROUND(AVG(CAST(fp.PitchX AS DECIMAL(9, 4))), 0) AS TINYINT) AS PitchX,
                   CAST(ROUND(AVG(CAST(fp.PitchY AS DECIMAL(9, 4))), 0) AS TINYINT) AS PitchY
            FROM dbo.FormationPositions fp
            WHERE fp.PositionId = pp.PositionId
              AND fp.PitchX IS NOT NULL
              AND fp.PitchY IS NOT NULL
        ) spot
    WHERE pp.PlayerId = @PlayerId
    ORDER BY pp.Quality DESC, pp.Id;
END
