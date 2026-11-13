CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayerTransferSwaps
    @LeagueTeamPlayerTransferId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by Id so the swapped players come back in the order they were added.
    SELECT Id, LeagueTeamPlayerTransferId, LeagueTeamPlayerId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamPlayerTransferSwaps
    WHERE LeagueTeamPlayerTransferId = @LeagueTeamPlayerTransferId
    ORDER BY Id;
END
