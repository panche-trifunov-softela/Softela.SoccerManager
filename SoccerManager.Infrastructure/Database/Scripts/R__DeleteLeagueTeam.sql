CREATE OR ALTER PROCEDURE dbo.DeleteLeagueTeam
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- A club leaves a league's roster only when nothing is attached to it there. The checks take range
    -- locks (UPDLOCK, HOLDLOCK) inside the caller's transaction, so nothing can be attached between the
    -- check and the delete.
    DECLARE @Exists BIT = CASE WHEN EXISTS (
        SELECT 1 FROM dbo.LeagueTeams WITH (UPDLOCK, HOLDLOCK) WHERE Id = @Id) THEN 1 ELSE 0 END;

    DECLARE @HasSquad BIT = CASE WHEN EXISTS (
        SELECT 1 FROM dbo.LeagueTeamPlayers WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId = @Id) THEN 1 ELSE 0 END;

    DECLARE @HasAppointments BIT = CASE WHEN EXISTS (
        SELECT 1 FROM dbo.LeagueTeamManagers WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId = @Id) THEN 1 ELSE 0 END;

    DECLARE @HasApplications BIT = CASE WHEN EXISTS (
        SELECT 1 FROM dbo.LeagueTeamManagerApplications WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId = @Id) THEN 1 ELSE 0 END;

    DECLARE @HasStandings BIT = CASE WHEN EXISTS (
        SELECT 1 FROM dbo.Standings WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId = @Id) THEN 1 ELSE 0 END;

    DECLARE @HasMatches BIT = CASE WHEN EXISTS (
        SELECT 1 FROM dbo.MatchTeamStatistics WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId = @Id)
        OR EXISTS (
        SELECT 1 FROM dbo.MatchTeamTactics WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId = @Id)
        OR EXISTS (
        SELECT 1 FROM dbo.MatchFormationPlayerPositions WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId = @Id) THEN 1 ELSE 0 END;

    DECLARE @Deleted BIT = 0;

    IF @Exists = 1
        AND @HasSquad = 0 AND @HasAppointments = 0 AND @HasApplications = 0 AND @HasStandings = 0 AND @HasMatches = 0
    BEGIN
        DELETE FROM dbo.LeagueTeams WHERE Id = @Id;

        SET @Deleted = 1;
    END

    SELECT @Deleted AS Deleted, @HasSquad AS HasSquad, @HasAppointments AS HasAppointments,
        @HasApplications AS HasApplications, @HasStandings AS HasStandings, @HasMatches AS HasMatches;
END
