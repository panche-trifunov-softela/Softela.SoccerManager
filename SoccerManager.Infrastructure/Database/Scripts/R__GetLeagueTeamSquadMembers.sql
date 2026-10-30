-- The squad of one league team for its manager, behind GET /api/league-teams/{id}/squad: each squad entry with the player's
-- date of birth, current rating, main position and national team, and the entry's squad number, value, weekly wage, morale
-- and condition. It does not check access: the query handler does that first, through IClubAccess.
-- Id is the squad entry (dbo.LeagueTeamPlayers.Id) and PlayerId the player, so a player registered twice (which V1_0_0_37
-- allows under a race) is listed twice, under two Ids.
-- The current rating (the dbo.PlayerRatings row with IsCurrent = 1, at most one by UX_PlayerRatings_PlayerId_Current) and
-- the national team are LEFT JOINs, so a player with neither is still listed, with NULLs.
-- The main position comes from dbo.GetPlayerMainPosition, and the order is kept identical to dbo.GetLeagueTeamSquad's: by
-- line and position, a player with no position last of all, then by rating, highest first, then name.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamSquadMembers
    @LeagueTeamId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT ltp.Id,
           p.Id AS PlayerId,
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
           ltp.TransfermarketValue AS Value,
           ltp.ContractSalaryPerWeek AS Wage,
           ltp.Morale,
           ltp.Condition
    FROM dbo.LeagueTeamPlayers ltp
        INNER JOIN dbo.Players p ON p.Id = ltp.PlayerId
        LEFT JOIN dbo.PlayerRatings cr ON cr.PlayerId = p.Id AND cr.IsCurrent = 1
        OUTER APPLY dbo.GetPlayerMainPosition(p.Id) mp
        LEFT JOIN dbo.Positions pos ON pos.Id = mp.PositionId
        LEFT JOIN dbo.NationalTeams nt ON nt.Id = p.NationalTeamId
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
