CREATE OR ALTER PROCEDURE dbo.DeleteUnstartedMatchFormationPlayerPositionsByLeagueTeamAndPlayer
    @LeagueTeamId INT,
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag. It clears a released player from the
    -- club's lineups for matches that have not started. Slots in started matches are kept as the record of
    -- who played. A slot reaches its player through the player position rating it points at.
    DELETE mfpp
    FROM dbo.MatchFormationPlayerPositions AS mfpp
    INNER JOIN dbo.PlayerPositions AS pp ON pp.Id = mfpp.PlayerPositionId
    INNER JOIN dbo.Matches AS m ON m.Id = mfpp.MatchId
    WHERE mfpp.LeagueTeamId = @LeagueTeamId
        AND pp.PlayerId = @PlayerId
        AND m.IsStarted = 0;
END
