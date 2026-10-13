CREATE OR ALTER PROCEDURE dbo.DeleteLeagueTeam
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- A club leaves a league's roster only when nothing is attached to it there. The checks take range
    -- locks (UPDLOCK, HOLDLOCK) inside the caller's transaction, so nothing can be attached between the
    -- check and the delete. Tables not yet keyed by LeagueTeamId are matched by their (LeagueId, TeamId).
    DECLARE @LeagueId INT;
    DECLARE @TeamId INT;

    SELECT @LeagueId = LeagueId, @TeamId = TeamId
    FROM dbo.LeagueTeams WITH (UPDLOCK, HOLDLOCK)
    WHERE Id = @Id;

    DECLARE @HasSquad BIT = CASE WHEN EXISTS (
        SELECT 1 FROM dbo.LeagueTeamPlayers WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId = @Id) THEN 1 ELSE 0 END;

    DECLARE @HasAppointments BIT = CASE WHEN EXISTS (
        SELECT 1 FROM dbo.LeagueTeamManagers WITH (UPDLOCK, HOLDLOCK) WHERE LeagueId = @LeagueId AND TeamId = @TeamId) THEN 1 ELSE 0 END;

    DECLARE @HasApplications BIT = CASE WHEN EXISTS (
        SELECT 1 FROM dbo.LeagueTeamManagerApplications WITH (UPDLOCK, HOLDLOCK) WHERE LeagueId = @LeagueId AND TeamId = @TeamId) THEN 1 ELSE 0 END;

    DECLARE @HasStandings BIT = CASE WHEN EXISTS (
        SELECT 1
        FROM dbo.Standings s WITH (UPDLOCK, HOLDLOCK)
            INNER JOIN dbo.Competitions c ON c.Id = s.CompetitionId
        WHERE c.LeagueId = @LeagueId AND s.TeamId = @TeamId) THEN 1 ELSE 0 END;

    DECLARE @HasMatches BIT = CASE WHEN EXISTS (
        SELECT 1
        FROM dbo.MatchTeamStatistics x WITH (UPDLOCK, HOLDLOCK)
            INNER JOIN dbo.Matches m ON m.Id = x.MatchId
            INNER JOIN dbo.Seasons se ON se.Id = m.SeasonId
        WHERE se.LeagueId = @LeagueId AND x.TeamId = @TeamId)
        OR EXISTS (
        SELECT 1
        FROM dbo.MatchTeamTactics x WITH (UPDLOCK, HOLDLOCK)
            INNER JOIN dbo.Matches m ON m.Id = x.MatchId
            INNER JOIN dbo.Seasons se ON se.Id = m.SeasonId
        WHERE se.LeagueId = @LeagueId AND x.TeamId = @TeamId)
        OR EXISTS (
        SELECT 1
        FROM dbo.MatchFormationPlayerPositions x WITH (UPDLOCK, HOLDLOCK)
            INNER JOIN dbo.Matches m ON m.Id = x.MatchId
            INNER JOIN dbo.Seasons se ON se.Id = m.SeasonId
        WHERE se.LeagueId = @LeagueId AND x.TeamId = @TeamId) THEN 1 ELSE 0 END;

    DECLARE @Deleted BIT = 0;

    IF @LeagueId IS NOT NULL
        AND @HasSquad = 0 AND @HasAppointments = 0 AND @HasApplications = 0 AND @HasStandings = 0 AND @HasMatches = 0
    BEGIN
        DELETE FROM dbo.LeagueTeams WHERE Id = @Id;

        SET @Deleted = 1;
    END

    SELECT @Deleted AS Deleted, @HasSquad AS HasSquad, @HasAppointments AS HasAppointments,
        @HasApplications AS HasApplications, @HasStandings AS HasStandings, @HasMatches AS HasMatches;
END
