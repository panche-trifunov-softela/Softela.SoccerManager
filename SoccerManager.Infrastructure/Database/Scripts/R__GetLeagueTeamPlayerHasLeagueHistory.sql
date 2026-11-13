-- Backs the cross-league move guard in UpdateLeagueTeamPlayerValidator: a squad row that has a transfer, a loan, a swap row or an
-- injury holds history that belongs to its own league (the clubs and seasons a deal names, the injuries recorded on it), so it cannot move to another league.
-- Any one of dbo.LeagueTeamPlayerTransfers, dbo.LeagueTeamPlayerLoans, dbo.LeagueTeamPlayerTransferSwaps and
-- dbo.LeagueTeamPlayerInjuries holding a row for @Id gives 1, else 0. An unknown @Id gives 0.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayerHasLeagueHistory
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CAST(CASE WHEN EXISTS (SELECT 1 FROM dbo.LeagueTeamPlayerTransfers WHERE LeagueTeamPlayerId = @Id)
        OR EXISTS (SELECT 1 FROM dbo.LeagueTeamPlayerLoans WHERE LeagueTeamPlayerId = @Id)
        OR EXISTS (SELECT 1 FROM dbo.LeagueTeamPlayerTransferSwaps WHERE LeagueTeamPlayerId = @Id)
        OR EXISTS (SELECT 1 FROM dbo.LeagueTeamPlayerInjuries WHERE LeagueTeamPlayerId = @Id) THEN 1 ELSE 0 END AS BIT) AS HasLeagueHistory;
END
