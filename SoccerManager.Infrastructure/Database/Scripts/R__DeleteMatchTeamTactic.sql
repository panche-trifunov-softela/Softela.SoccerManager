CREATE OR ALTER PROCEDURE dbo.DeleteMatchTeamTactic
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.MatchTeamTactics WHERE Id = @Id;
END
