CREATE OR ALTER PROCEDURE dbo.UpdateLeagueTeamPlayerLoan
    @Id                      INT,
    @LeagueTeamPlayerId      INT,
    @ToLeagueTeamId          INT,
    @SeasonId                INT,
    @LoanFee                 DECIMAL(18,2),
    @SeasonsLength           INT,
    @BuyOptionValue          DECIMAL(18,2),
    @SalaryCoverageInPercent INT,
    @Status                  TINYINT,
    @ModifiedAt              DATETIME2(7),
    @ModifiedBy              UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.LeagueTeamPlayerLoans
    SET LeagueTeamPlayerId      = @LeagueTeamPlayerId,
        ToLeagueTeamId          = @ToLeagueTeamId,
        SeasonId                = @SeasonId,
        LoanFee                 = @LoanFee,
        SeasonsLength           = @SeasonsLength,
        BuyOptionValue          = @BuyOptionValue,
        SalaryCoverageInPercent = @SalaryCoverageInPercent,
        Status                  = @Status,
        ModifiedAt              = @ModifiedAt,
        ModifiedBy              = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
