-- Backs the one-pending-bid rule in PlayerDealService.GetBidTargetAsync: a club may have one pending bid per player, whether
-- it is a transfer or a loan, and whichever registration of the player it names. A pending transfer or loan (Status 1,
-- TransferStatus.Pending) into @LeagueTeamId for a squad row of @PlayerId gives 1, else 0.
-- It runs inside the caller's transaction and first locks the bidding club's own row (UPDLOCK, HOLDLOCK), so two bids from one
-- club run one after the other and the second sees the first. One row is locked instead of ranges of the deal tables, which
-- keeps the lock small and out of the way of the squad-row delete cascade. LeagueTeams is locked first, as
-- dbo.DeleteLeagueTeam does, so the two never wait on each other in opposite orders. The deal tables are read without locks.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamHasPendingBid
    @LeagueTeamId INT,
    @PlayerId     INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LockedLeagueTeamId INT;

    SELECT @LockedLeagueTeamId = Id
    FROM dbo.LeagueTeams WITH (UPDLOCK, HOLDLOCK)
    WHERE Id = @LeagueTeamId;

    SELECT CAST(CASE WHEN EXISTS (
            SELECT 1
            FROM dbo.LeagueTeamPlayerTransfers t
            INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.Id = t.LeagueTeamPlayerId
            WHERE t.ToLeagueTeamId = @LeagueTeamId AND t.Status = 1 AND ltp.PlayerId = @PlayerId)
        OR EXISTS (
            SELECT 1
            FROM dbo.LeagueTeamPlayerLoans l
            INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.Id = l.LeagueTeamPlayerId
            WHERE l.ToLeagueTeamId = @LeagueTeamId AND l.Status = 1 AND ltp.PlayerId = @PlayerId) THEN 1 ELSE 0 END AS BIT) AS HasPendingBid;
END
