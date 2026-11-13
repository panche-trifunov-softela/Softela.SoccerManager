CREATE OR ALTER PROCEDURE dbo.UpdateLeagueTeamPlayerTransferSwap
    @Id                         INT,
    @LeagueTeamPlayerTransferId INT,
    @LeagueTeamPlayerId         INT,
    @ModifiedAt                 DATETIME2(7),
    @ModifiedBy                 UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.LeagueTeamPlayerTransferSwaps
    SET LeagueTeamPlayerTransferId = @LeagueTeamPlayerTransferId,
        LeagueTeamPlayerId         = @LeagueTeamPlayerId,
        ModifiedAt                 = @ModifiedAt,
        ModifiedBy                 = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
