CREATE OR ALTER PROCEDURE dbo.DeletePosition
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.Positions WHERE Id = @Id;
END
