CREATE OR ALTER PROCEDURE dbo.DeletePlayerPosition
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.PlayerPositions WHERE Id = @Id;
END
