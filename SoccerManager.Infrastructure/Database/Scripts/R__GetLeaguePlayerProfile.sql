-- One player's profile in a league, behind GET /api/league-teams/{id}/league-players/{playerId}, as two result sets:
-- the player's header row, then the player's positions. It does not check access: the query handler does that first, through
-- IClubAccess.
-- The league is the league of @LeagueTeamId, and the player may be registered at any club of it, rival clubs included. The
-- header is that registration: the club's own (@LeagueTeamId) when the player has one, else the one at another club of the same
-- league. There is no unique key on (LeagueTeamId, PlayerId) (V1_0_0_37 dropped it), so the lowest dbo.LeagueTeamPlayers.Id
-- comes first and also settles a duplicate inside one club. No row at all means the player has no registration at any club of
-- the league, or the league team is unknown.
-- Value, Wage, ContractLength, Morale, MoraleConcern, Condition and IsSuspended are the registration's own. ContractLength is
-- the seasons left on the contract, as stored. MoraleConcern is NULL when there is none. IsSuspended is the domestic competition
-- suspension flag, the one lineups read. The player's details, current rating, main position and national team come from the
-- same sources as dbo.GetLeagueTeamSquadMembers; the current rating and the national team are LEFT JOINs, so a player with
-- neither still gets a profile, with NULLs.
-- The positions are every dbo.PlayerPositions row of the player, best quality first, the main position therefore first. Each
-- carries the spot it is drawn at on a pitch: the average PitchX and PitchY of the starter slots in dbo.FormationPositions that
-- use that position, rounded to a whole percentage. Only starter slots have coordinates (V1_0_0_50), and a position no formation
-- uses gets NULL and is not drawn. AVG over a TINYINT returns an INT and truncates, hence the widening to DECIMAL first. The
-- aggregate stays inside the OUTER APPLY and names no outer column: SQL Server rejects an aggregate over an outer reference
-- (Msg 8124).
CREATE OR ALTER PROCEDURE dbo.GetLeaguePlayerProfile
    @LeagueTeamId INT,
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
           ltp.Id,
           ltp.PlayerId,
           ltp.LeagueTeamId,
           t.Name AS TeamName,
           t.LogoUrl AS TeamLogoUrl,
           p.Name,
           p.ImageUrl,
           p.DateOfBirth,
           ltp.SquadNumber,
           mp.PositionId,
           pos.Name AS PositionName,
           pos.ShortName AS PositionShortName,
           p.NationalTeamId,
           nt.Name AS NationalTeamName,
           nt.LogoUrl AS NationalTeamLogoUrl,
           cr.Rating,
           ltp.TransfermarketValue AS [Value],
           ltp.ContractSalaryPerWeek AS Wage,
           ltp.ContractLength,
           ltp.Morale,
           ltp.MoraleConcern,
           ltp.Condition,
           ltp.IsSuspendedDomesticCompetition AS IsSuspended
    FROM dbo.LeagueTeams anchor
        INNER JOIN dbo.LeagueTeams lt ON lt.LeagueId = anchor.LeagueId
        INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.LeagueTeamId = lt.Id AND ltp.PlayerId = @PlayerId
        INNER JOIN dbo.Teams t ON t.Id = lt.TeamId
        INNER JOIN dbo.Players p ON p.Id = ltp.PlayerId
        LEFT JOIN dbo.NationalTeams nt ON nt.Id = p.NationalTeamId
        LEFT JOIN dbo.PlayerRatings cr ON cr.PlayerId = p.Id AND cr.IsCurrent = 1
        OUTER APPLY dbo.GetPlayerMainPosition(p.Id) mp
        LEFT JOIN dbo.Positions pos ON pos.Id = mp.PositionId
    WHERE anchor.Id = @LeagueTeamId
    ORDER BY CASE WHEN ltp.LeagueTeamId = @LeagueTeamId THEN 0 ELSE 1 END,
             ltp.Id;

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
