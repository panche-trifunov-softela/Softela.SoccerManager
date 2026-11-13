CREATE OR ALTER PROCEDURE dbo.DeleteLeagueTeamPlayerTransferSwap
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag.
    DELETE FROM dbo.LeagueTeamPlayerTransferSwaps WHERE Id = @Id;
END
