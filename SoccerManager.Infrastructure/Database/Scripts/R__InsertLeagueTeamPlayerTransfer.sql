CREATE OR ALTER PROCEDURE dbo.InsertLeagueTeamPlayerTransfer
    @LeagueTeamPlayerId  INT,
    @FromLeagueTeamId    INT,
    @ToLeagueTeamId      INT,
    @SeasonId            INT,
    @TransferFee         DECIMAL(18,2),
    @BuybackValue        DECIMAL(18,2),
    @BuybackValidSeasons INT,
    @Status              TINYINT,
    @CreatedAt           DATETIME2(7),
    @ModifiedAt          DATETIME2(7),
    @CreatedBy           UNIQUEIDENTIFIER,
    @ModifiedBy          UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.LeagueTeamPlayerTransfers (LeagueTeamPlayerId, FromLeagueTeamId, ToLeagueTeamId, SeasonId, TransferFee, BuybackValue, BuybackValidSeasons, Status, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueTeamPlayerId, @FromLeagueTeamId, @ToLeagueTeamId, @SeasonId, @TransferFee, @BuybackValue, @BuybackValidSeasons, @Status, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
