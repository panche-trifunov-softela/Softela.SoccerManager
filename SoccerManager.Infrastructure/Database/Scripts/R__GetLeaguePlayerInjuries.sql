-- One player's injury history in a league, behind GET /api/league-teams/{id}/league-players/{playerId}/injuries, as two
-- result sets: whether the player is registered in the league, then the player's injuries. It does not check access: the
-- query handler does that first, through IClubAccess.
-- The league is the league of @LeagueTeamId, found as in dbo.GetLeaguePlayerProfile. The first result set is one row with
-- one column, IsRegistered: 1 when the player has a dbo.LeagueTeamPlayers row at any club of that league, rival clubs included,
-- else 0. An unknown @LeagueTeamId gives 0. The handler answers a 0 with a 404, as the profile read does.
-- The second result set is every injury recorded on the player's squad rows (dbo.LeagueTeamPlayers) at the clubs of that same
-- league, newest first (InjuryStartDate, then Id, as two injuries can start at the same moment). An injury names no club, so a row
-- carries only its Id, Name, InjuryStartDate, InjuryEndDate (both UTC) and InjuryScale (a SoccerManager.Domain.Enums.InjuryScale
-- value). An injury recorded on a squad row at a club of another league never appears. The second result set is returned
-- whatever the first one says.
CREATE OR ALTER PROCEDURE dbo.GetLeaguePlayerInjuries
    @LeagueTeamId INT,
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CAST(CASE WHEN EXISTS (
        SELECT 1
        FROM dbo.LeagueTeams anchor
            INNER JOIN dbo.LeagueTeams lt ON lt.LeagueId = anchor.LeagueId
            INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.LeagueTeamId = lt.Id AND ltp.PlayerId = @PlayerId
        WHERE anchor.Id = @LeagueTeamId) THEN 1 ELSE 0 END AS BIT) AS IsRegistered;

    SELECT inj.Id,
           inj.Name,
           inj.InjuryStartDate,
           inj.InjuryEndDate,
           inj.InjuryScale
    FROM dbo.LeagueTeams anchor
        INNER JOIN dbo.LeagueTeams lt ON lt.LeagueId = anchor.LeagueId
        INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.LeagueTeamId = lt.Id AND ltp.PlayerId = @PlayerId
        INNER JOIN dbo.LeagueTeamPlayerInjuries inj ON inj.LeagueTeamPlayerId = ltp.Id
    WHERE anchor.Id = @LeagueTeamId
    ORDER BY inj.InjuryStartDate DESC,
             inj.Id DESC;
END
