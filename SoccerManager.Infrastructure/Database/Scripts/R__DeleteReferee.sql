CREATE OR ALTER PROCEDURE dbo.DeleteReferee
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.Referees WHERE Id = @Id;
END
