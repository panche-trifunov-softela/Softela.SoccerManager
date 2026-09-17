CREATE OR ALTER PROCEDURE dbo.DeleteManager
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.Managers WHERE Id = @Id;
END
