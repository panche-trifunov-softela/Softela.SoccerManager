CREATE OR ALTER PROCEDURE dbo.DeleteLeagueTeamManagerApplication
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.LeagueTeamManagerApplications WHERE Id = @Id;
END
