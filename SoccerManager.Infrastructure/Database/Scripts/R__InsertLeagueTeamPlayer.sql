CREATE OR ALTER PROCEDURE dbo.InsertLeagueTeamPlayer
    @LeagueId                 INT,
    @TeamId                   INT,
    @PlayerId                 INT,
    @ContractLength           INT,
    @ContractSalaryPerWeek    BIGINT,
    @SquadNumber              INT,
    @FansFavoritePlayer       BIT,
    @Morale                   TINYINT,
    @TransfermarketValue      BIGINT,
    @WantedStarterAppearances INT,
    @WantedTotalAppearances   INT,
    @CreatedAt                DATETIME2(7),
    @ModifiedAt               DATETIME2(7),
    @CreatedBy                UNIQUEIDENTIFIER,
    @ModifiedBy               UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.LeagueTeamPlayers (LeagueId, TeamId, PlayerId, ContractLength, ContractSalaryPerWeek, SquadNumber, FansFavoritePlayer, Morale, TransfermarketValue, WantedStarterAppearances, WantedTotalAppearances, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueId, @TeamId, @PlayerId, @ContractLength, @ContractSalaryPerWeek, @SquadNumber, @FansFavoritePlayer, @Morale, @TransfermarketValue, @WantedStarterAppearances, @WantedTotalAppearances, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
