CREATE OR ALTER PROCEDURE dbo.DeleteLeagueTeamPlayerTransfer
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- This is a hard delete; the table has no soft-delete flag. The transfer's swap rows go with it through
    -- FK_LeagueTeamPlayerTransferSwaps_LeagueTeamPlayerTransfers.
    DELETE FROM dbo.LeagueTeamPlayerTransfers WHERE Id = @Id;
END
