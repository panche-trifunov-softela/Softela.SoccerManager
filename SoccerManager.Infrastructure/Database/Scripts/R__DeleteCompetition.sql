CREATE OR ALTER PROCEDURE dbo.DeleteCompetition
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.Competitions WHERE Id = @Id;
END
