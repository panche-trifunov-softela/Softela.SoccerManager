CREATE OR ALTER PROCEDURE dbo.DeleteUnstartedMatchFormationPlayerPositionSubstitutionsByLeagueTeamAndPlayer
    @LeagueTeamId INT,
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag. It clears a released player from the
    -- club's substitution lineups for matches that have not started, as
    -- dbo.DeleteUnstartedMatchFormationPlayerPositionsByLeagueTeamAndPlayer does for the main lineups.
    -- A slot reaches its player through the player position rating it points at.
    DELETE mfpps
    FROM dbo.MatchFormationPlayerPositionSubstitutions AS mfpps
    INNER JOIN dbo.PlayerPositions AS pp ON pp.Id = mfpps.PlayerPositionId
    INNER JOIN dbo.Matches AS m ON m.Id = mfpps.MatchId
    WHERE mfpps.LeagueTeamId = @LeagueTeamId
        AND pp.PlayerId = @PlayerId
        AND m.IsStarted = 0;
END
