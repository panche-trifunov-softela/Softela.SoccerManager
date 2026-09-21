CREATE OR ALTER PROCEDURE dbo.DeleteFormationPosition
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.FormationPositions WHERE Id = @Id;
END
