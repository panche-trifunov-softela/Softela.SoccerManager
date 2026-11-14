CREATE OR ALTER PROCEDURE dbo.DeleteMatchPlayerAbsence
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.MatchPlayerAbsences WHERE Id = @Id;
END
