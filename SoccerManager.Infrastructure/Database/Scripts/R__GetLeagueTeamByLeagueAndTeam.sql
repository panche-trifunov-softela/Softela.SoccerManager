CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamByLeagueAndTeam
    @LeagueId INT,
    @TeamId   INT
AS
BEGIN
    SET NOCOUNT ON;

    -- UQ_LeagueTeams_LeagueId_TeamId guarantees at most one row for the pair.
    SELECT Id, LeagueId, TeamId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeams
    WHERE LeagueId = @LeagueId AND TeamId = @TeamId;
END
