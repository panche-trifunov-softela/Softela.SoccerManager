CREATE OR ALTER PROCEDURE dbo.DeleteMatchTeamStatistic
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.MatchTeamStatistics WHERE Id = @Id;
END
