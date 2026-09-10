CREATE OR ALTER PROCEDURE dbo.DeleteStanding
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.Standings WHERE Id = @Id;
END
