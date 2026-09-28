CREATE OR ALTER PROCEDURE dbo.DeleteNationalTeam
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.NationalTeams WHERE Id = @Id;
END
