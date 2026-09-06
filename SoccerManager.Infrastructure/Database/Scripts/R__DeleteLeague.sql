CREATE OR ALTER PROCEDURE dbo.DeleteLeague
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.Leagues WHERE Id = @Id;
END
