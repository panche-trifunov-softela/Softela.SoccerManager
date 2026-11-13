CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayerTransferSwapById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueTeamPlayerTransferId, LeagueTeamPlayerId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamPlayerTransferSwaps
    WHERE Id = @Id;
END
