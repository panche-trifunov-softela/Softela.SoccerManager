CREATE OR ALTER PROCEDURE dbo.DeleteMatchFormationPlayerPositionSubstitution
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.MatchFormationPlayerPositionSubstitutions WHERE Id = @Id;
END
