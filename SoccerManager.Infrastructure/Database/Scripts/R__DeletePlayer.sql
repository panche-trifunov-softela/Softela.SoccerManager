CREATE OR ALTER PROCEDURE dbo.DeletePlayer
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.Players WHERE Id = @Id;
END
