CREATE OR ALTER PROCEDURE dbo.UpdateLeagueTeamPlayerTransfer
    @Id                  INT,
    @LeagueTeamPlayerId  INT,
    @FromLeagueTeamId    INT,
    @ToLeagueTeamId      INT,
    @SeasonId            INT,
    @TransferFee         DECIMAL(18,2),
    @BuybackValue        DECIMAL(18,2),
    @BuybackValidSeasons INT,
    @Status              TINYINT,
    @ModifiedAt          DATETIME2(7),
    @ModifiedBy          UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.LeagueTeamPlayerTransfers
    SET LeagueTeamPlayerId  = @LeagueTeamPlayerId,
        FromLeagueTeamId    = @FromLeagueTeamId,
        ToLeagueTeamId      = @ToLeagueTeamId,
        SeasonId            = @SeasonId,
        TransferFee         = @TransferFee,
        BuybackValue        = @BuybackValue,
        BuybackValidSeasons = @BuybackValidSeasons,
        Status              = @Status,
        ModifiedAt          = @ModifiedAt,
        ModifiedBy          = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
