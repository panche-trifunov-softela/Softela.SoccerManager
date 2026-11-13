CREATE OR ALTER PROCEDURE dbo.InsertLeagueTeamPlayerLoan
    @LeagueTeamPlayerId      INT,
    @ToLeagueTeamId          INT,
    @SeasonId                INT,
    @LoanFee                 DECIMAL(18,2),
    @SeasonsLength           INT,
    @BuyOptionValue          DECIMAL(18,2),
    @SalaryCoverageInPercent INT,
    @Status                  TINYINT,
    @CreatedAt               DATETIME2(7),
    @ModifiedAt              DATETIME2(7),
    @CreatedBy               UNIQUEIDENTIFIER,
    @ModifiedBy              UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.LeagueTeamPlayerLoans (LeagueTeamPlayerId, ToLeagueTeamId, SeasonId, LoanFee, SeasonsLength, BuyOptionValue, SalaryCoverageInPercent, Status, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@LeagueTeamPlayerId, @ToLeagueTeamId, @SeasonId, @LoanFee, @SeasonsLength, @BuyOptionValue, @SalaryCoverageInPercent, @Status, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
