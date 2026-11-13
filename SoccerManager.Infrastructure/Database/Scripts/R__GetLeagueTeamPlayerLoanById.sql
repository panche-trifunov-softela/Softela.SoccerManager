CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayerLoanById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, LeagueTeamPlayerId, ToLeagueTeamId, SeasonId, LoanFee, SeasonsLength, BuyOptionValue, SalaryCoverageInPercent, Status, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamPlayerLoans
    WHERE Id = @Id;
END
