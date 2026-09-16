CREATE OR ALTER PROCEDURE dbo.DeleteLeagueTeamManager
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.LeagueTeamManagers WHERE Id = @Id;
END
