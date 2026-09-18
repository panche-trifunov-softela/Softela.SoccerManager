CREATE OR ALTER PROCEDURE dbo.DeleteLeagueTeamPlayer
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.LeagueTeamPlayers WHERE Id = @Id;
END
