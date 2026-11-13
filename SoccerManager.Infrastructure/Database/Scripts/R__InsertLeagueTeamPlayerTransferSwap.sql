CREATE OR ALTER PROCEDURE dbo.InsertLeagueTeamPlayerTransferSwap
    @LeagueTeamPlayerTransferId INT,
    @LeagueTeamPlayerId         INT,
    @CreatedAt                  DATETIME2(7),
    @ModifiedAt                 DATETIME2(7),
    @CreatedBy                  UNIQUEIDENTIFIER,
    @ModifiedBy                 UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.LeagueTeamPlayerTransferSwaps (LeagueTeamPlayerTransferId, LeagueTeamPlayerId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueTeamPlayerTransferId, @LeagueTeamPlayerId, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
