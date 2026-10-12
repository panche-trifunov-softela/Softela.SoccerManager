CREATE OR ALTER PROCEDURE dbo.DeleteLeagueTeam
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.LeagueTeams WHERE Id = @Id;
END
