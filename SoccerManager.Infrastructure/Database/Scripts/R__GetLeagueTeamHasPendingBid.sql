-- Backs the one-pending-bid rule in PlayerDealService.GetBidTargetAsync: a club may have one pending bid per player, whether
-- it is a transfer or a loan, and whichever registration of the player it names. A pending transfer or loan (Status 1,
-- TransferStatus.Pending) into @LeagueTeamId for a squad row of @PlayerId gives 1, else 0.
-- It runs inside the caller's transaction and first locks the LeagueTeams rows of the bidding club (@LeagueTeamId) and the
-- selling or lending club (@SellerLeagueTeamId), each WITH (UPDLOCK, HOLDLOCK), so two bids from one club run one after the
-- other and the second sees the first. The seller is locked too because an accept on the seller's player takes the seller's
-- lock (dbo.AnswerLeagueTeamPlayerTransferBid, dbo.AnswerLeagueTeamPlayerLoanBid), so a bid and an accept on the same player run
-- one after the other: the bid sees the agreed deal (dbo.GetLeagueTeamPlayerHasAgreedDeal, read after this lock), or the accept
-- rejects the bid. The two rows are locked in ascending Id order, one single-row statement each, and the second is skipped when
-- the two clubs are the same, so two callers never wait on each other in opposite orders. One row per club is locked instead of
-- ranges of the deal tables, which keeps the lock small and out of the way of the squad-row delete cascade. LeagueTeams is locked
-- first, as dbo.DeleteLeagueTeam does, so the two never wait on each other in opposite orders. The deal tables are read without
-- locks.
CREATE OR ALTER PROCEDURE dbo.GetLeagueTeamHasPendingBid
    @LeagueTeamId       INT,
    @SellerLeagueTeamId INT,
    @PlayerId           INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LockedLeagueTeamId INT;
    DECLARE @FirstLockId INT;
    DECLARE @SecondLockId INT;

    -- The lower Id is locked first; the second lock is skipped when the bidder is the seller.
    SET @FirstLockId = CASE WHEN @SellerLeagueTeamId < @LeagueTeamId THEN @SellerLeagueTeamId ELSE @LeagueTeamId END;
    SET @SecondLockId = CASE WHEN @SellerLeagueTeamId < @LeagueTeamId THEN @LeagueTeamId
                             WHEN @SellerLeagueTeamId > @LeagueTeamId THEN @SellerLeagueTeamId
                        END;

    SELECT @LockedLeagueTeamId = Id
    FROM dbo.LeagueTeams WITH (UPDLOCK, HOLDLOCK)
    WHERE Id = @FirstLockId;

    IF @SecondLockId IS NOT NULL
    BEGIN
        SELECT @LockedLeagueTeamId = Id
        FROM dbo.LeagueTeams WITH (UPDLOCK, HOLDLOCK)
        WHERE Id = @SecondLockId;
    END

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
