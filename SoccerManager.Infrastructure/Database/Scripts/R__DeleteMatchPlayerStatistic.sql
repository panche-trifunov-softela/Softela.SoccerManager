CREATE OR ALTER PROCEDURE dbo.DeleteMatchPlayerStatistic
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.MatchPlayerStatistics WHERE Id = @Id;
END
