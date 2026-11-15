-- A team is deleted only when no league still has standings, match rows or deals for one of its roster rows;
-- otherwise nothing is deleted. A transfer from or to one of its clubs blocks even when it would cascade away with
-- its squad row, as in dbo.DeleteLeagueTeam, so no sale is lost with the team. The team's roster rows in every league,
-- with their squads, appointments, applications, injuries and the loans it made, cascade away with it.
-- Returns one row: Deleted and the three blocker flags. Every check reads with UPDLOCK, HOLDLOCK inside the
-- caller's transaction, so nothing can be added between the check and the delete.
CREATE OR ALTER PROCEDURE dbo.DeleteTeam
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Exists BIT = 0;
    DECLARE @Deleted BIT = 0;
    DECLARE @HasStandings BIT = 0;
    DECLARE @HasMatches BIT = 0;
    DECLARE @HasTransfers BIT = 0;
    DECLARE @Roster TABLE (Id INT NOT NULL PRIMARY KEY);

    -- Locking the team also makes a concurrent roster insert for it wait on its foreign-key check.
    IF EXISTS (SELECT 1 FROM dbo.Teams WITH (UPDLOCK, HOLDLOCK) WHERE Id = @Id)
    BEGIN
        SET @Exists = 1;
    END

    -- The team's roster rows in every league, read once and used by every check below.
    INSERT INTO @Roster (Id)
    SELECT Id FROM dbo.LeagueTeams WITH (UPDLOCK, HOLDLOCK) WHERE TeamId = @Id;

    -- Cup standings count as well as division standings.
    IF EXISTS (
        SELECT 1 FROM dbo.Standings WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId IN (SELECT Id FROM @Roster))
    BEGIN
        SET @HasStandings = 1;
    END

    IF EXISTS (
        SELECT 1 FROM dbo.MatchTeamStatistics WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId IN (SELECT Id FROM @Roster))
        OR EXISTS (
        SELECT 1 FROM dbo.MatchTeamTactics WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId IN (SELECT Id FROM @Roster))
        OR EXISTS (
        SELECT 1 FROM dbo.MatchFormationPlayerPositions WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId IN (SELECT Id FROM @Roster))
        OR EXISTS (
        SELECT 1 FROM dbo.MatchTeamTacticSubstitutions WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId IN (SELECT Id FROM @Roster))
        OR EXISTS (
        SELECT 1 FROM dbo.MatchFormationPlayerPositionSubstitutions WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId IN (SELECT Id FROM @Roster))
        OR EXISTS (
        SELECT 1 FROM dbo.MatchPlayerStatistics WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId IN (SELECT Id FROM @Roster))
        OR EXISTS (
        SELECT 1 FROM dbo.PenaltiesAfterExtraTime WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId IN (SELECT Id FROM @Roster))
        OR EXISTS (
        SELECT 1 FROM dbo.MatchPlayerAbsences WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId IN (SELECT Id FROM @Roster))
    BEGIN
        SET @HasMatches = 1;
    END

    -- A swap names a squad row, and a squad row can move to another club, so the swap check goes through the
    -- squad rows of the roster rows instead of the transfer's own clubs.
    IF EXISTS (
        SELECT 1 FROM dbo.LeagueTeamPlayerTransfers WITH (UPDLOCK, HOLDLOCK)
        WHERE FromLeagueTeamId IN (SELECT Id FROM @Roster) OR ToLeagueTeamId IN (SELECT Id FROM @Roster))
        OR EXISTS (
        SELECT 1 FROM dbo.LeagueTeamPlayerLoans WITH (UPDLOCK, HOLDLOCK) WHERE ToLeagueTeamId IN (SELECT Id FROM @Roster))
        OR EXISTS (
        SELECT 1 FROM dbo.LeagueTeamPlayerTransferSwaps WITH (UPDLOCK, HOLDLOCK)
        WHERE LeagueTeamPlayerId IN (
            SELECT Id FROM dbo.LeagueTeamPlayers WITH (UPDLOCK, HOLDLOCK) WHERE LeagueTeamId IN (SELECT Id FROM @Roster)))
    BEGIN
        SET @HasTransfers = 1;
    END

    IF @Exists = 1 AND @HasStandings = 0 AND @HasMatches = 0 AND @HasTransfers = 0
    BEGIN
        DELETE FROM dbo.Teams WHERE Id = @Id;

        SET @Deleted = 1;
    END

    SELECT @Deleted AS Deleted, @HasStandings AS HasStandings, @HasMatches AS HasMatches,
        @HasTransfers AS HasTransfers;
END
