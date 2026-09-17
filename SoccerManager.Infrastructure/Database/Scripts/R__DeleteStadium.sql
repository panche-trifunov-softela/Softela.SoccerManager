CREATE OR ALTER PROCEDURE dbo.DeleteStadium
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.Stadiums WHERE Id = @Id;
END
