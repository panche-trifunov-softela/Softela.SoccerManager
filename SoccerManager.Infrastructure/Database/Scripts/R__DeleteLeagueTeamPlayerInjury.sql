CREATE OR ALTER PROCEDURE dbo.DeleteLeagueTeamPlayerInjury
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.LeagueTeamPlayerInjuries WHERE Id = @Id;
END
