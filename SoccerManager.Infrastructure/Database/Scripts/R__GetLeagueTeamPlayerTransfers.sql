CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayerTransfers
    @LeagueTeamPlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by Id DESC so the player's latest transfer comes back first.
    SELECT Id, LeagueTeamPlayerId, FromLeagueTeamId, ToLeagueTeamId, SeasonId, TransferFee, BuybackValue, BuybackValidSeasons, Status, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamPlayerTransfers
    WHERE LeagueTeamPlayerId = @LeagueTeamPlayerId
    ORDER BY Id DESC;
END
