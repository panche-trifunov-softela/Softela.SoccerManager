CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayerLoans
    @LeagueTeamPlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by Id DESC so the player's latest loan comes back first.
    SELECT Id, LeagueTeamPlayerId, ToLeagueTeamId, SeasonId, LoanFee, SeasonsLength, BuyOptionValue, SalaryCoverageInPercent, Status, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.LeagueTeamPlayerLoans
    WHERE LeagueTeamPlayerId = @LeagueTeamPlayerId
    ORDER BY Id DESC;
END
