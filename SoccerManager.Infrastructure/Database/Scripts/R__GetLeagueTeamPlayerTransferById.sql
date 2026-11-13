CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayerTransferById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueTeamPlayerId, FromLeagueTeamId, ToLeagueTeamId, SeasonId, TransferFee, BuybackValue, BuybackValidSeasons, Status, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamPlayerTransfers
    WHERE Id = @Id;
END
