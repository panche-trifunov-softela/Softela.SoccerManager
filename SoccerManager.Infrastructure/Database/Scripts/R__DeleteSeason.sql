CREATE OR ALTER PROCEDURE dbo.DeleteSeason
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.Seasons WHERE Id = @Id;
END
