CREATE OR ALTER PROCEDURE dbo.DeleteMatchFormationPlayerPosition
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.MatchFormationPlayerPositions WHERE Id = @Id;
END
