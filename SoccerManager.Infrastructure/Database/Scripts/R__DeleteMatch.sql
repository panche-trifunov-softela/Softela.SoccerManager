CREATE OR ALTER PROCEDURE dbo.DeleteMatch
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.Matches WHERE Id = @Id;
END
