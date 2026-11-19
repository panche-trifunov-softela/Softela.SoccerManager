-- Backs the agreed-deal rule in PlayerDealService: a player whose deal is agreed takes no new bid and cannot be offered in a
-- swap. One row with one BIT column, HasAgreedDeal: 1 when the squad row @LeagueTeamPlayerId (dbo.LeagueTeamPlayers) is in an
-- agreed deal, else 0. Status 2 is TransferStatus.Accepted, a deal agreed and not carried out yet; a completed one (Status 4)
-- is not an agreed deal, the player has moved. A squad row is in an agreed deal when a transfer with Status 2 is on it, when a
-- swap row of a transfer with Status 2 names it, or when a loan with Status 2 is on it. An unknown row gives 0.
-- It takes no lock. It runs inside the place-bid transaction after dbo.GetLeagueTeamHasPendingBid has locked the bidding and the
-- selling club, and every accept that can make a row agreed locks that row's club (dbo.AnswerLeagueTeamPlayerTransferBid and
-- dbo.AnswerLeagueTeamPlayerLoanBid lock the selling or lending club and the bidding club), so the answer cannot change under it.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamPlayerHasAgreedDeal
    @LeagueTeamPlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CAST(CASE WHEN EXISTS (
            SELECT 1
            FROM dbo.LeagueTeamPlayerTransfers t
            WHERE t.Status = 2 AND t.LeagueTeamPlayerId = @LeagueTeamPlayerId)
        OR EXISTS (
            SELECT 1
            FROM dbo.LeagueTeamPlayerTransferSwaps sw
            INNER JOIN dbo.LeagueTeamPlayerTransfers t ON t.Id = sw.LeagueTeamPlayerTransferId
            WHERE t.Status = 2 AND sw.LeagueTeamPlayerId = @LeagueTeamPlayerId)
        OR EXISTS (
            SELECT 1
            FROM dbo.LeagueTeamPlayerLoans l
            WHERE l.Status = 2 AND l.LeagueTeamPlayerId = @LeagueTeamPlayerId) THEN 1 ELSE 0 END AS BIT) AS HasAgreedDeal;
END
