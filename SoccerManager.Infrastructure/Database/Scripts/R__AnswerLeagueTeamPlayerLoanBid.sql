-- Answers one pending loan bid on a player of @LeagueTeamId, behind PUT /api/league-teams/{id}/league-players/{playerId}/
-- loan-bids/{bidId}. @Status is a TransferStatus value: 2 Accepted or 3 Rejected (1 is Pending). It gives the bid's Id when the
-- answer was recorded and 0 when nothing was answered: no such bid, a bid that is no longer pending, or a bid on another
-- player's squad row or at another club. dbo.LeagueTeamPlayerLoans has no lender column, so the squad row alone says whose
-- player it is. It does not check access: the command handler does that first, through IClubAccess.
-- An accepted bid only records the agreement: the player stays and no squad row, lineup or other table changes. A later
-- Generator step completes accepted deals. Accepting does reject (Status 3) every OTHER pending bid on the player: the other
-- pending loans on the squad row, the pending transfers on it, and the pending transfers that offer it in a swap. That covers
-- the bids pending at that moment only: a bid placed later is not refused here, and a later step that completes accepted deals
-- settles that. The answered bid is left out of the loans' rejection; it is no longer pending anyway.
-- It runs inside the caller's transaction and first locks the LeagueTeams rows of both clubs of the deal, the lending club
-- (@LeagueTeamId) and the bidding club (the bid's ToLeagueTeamId, read before the lock; NULL when there is no such bid), each
-- WITH (UPDLOCK, HOLDLOCK), as dbo.GetLeagueTeamHasPendingBid does for a bidder. Every squad row of a deal belongs to its
-- lender or to its bidder, so two answers that could touch each other's bids always share one of the two clubs, and holding
-- both locks makes them run one after the other: the second sees the first one's result and gets 0 or finds the other bid
-- already rejected, instead of both succeeding or waiting on each other's row locks. The two rows are locked in ascending Id
-- order, one single-row statement each, so two answers never wait on each other in opposite orders. LeagueTeams is locked
-- first, as dbo.DeleteLeagueTeam and dbo.GetLeagueTeamHasPendingBid do, so those never wait on this in opposite orders either.
CREATE OR ALTER PROCEDURE dbo.AnswerLeagueTeamPlayerLoanBid
    @Id           INT,
    @LeagueTeamId INT,
    @PlayerId     INT,
    @Status       TINYINT,
    @ModifiedAt   DATETIME2(7),
    @ModifiedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LockedLeagueTeamId INT;
    DECLARE @Answered INT;
    DECLARE @DealLeagueTeamPlayerId INT;
    DECLARE @BidderLeagueTeamId INT;
    DECLARE @FirstLockId INT;
    DECLARE @SecondLockId INT;

    SELECT @BidderLeagueTeamId = ToLeagueTeamId
    FROM dbo.LeagueTeamPlayerLoans
    WHERE Id = @Id;

    -- The lower Id is locked first; the second lock is skipped when there is no bidder or the bidder is the lending club.
    SET @FirstLockId = CASE WHEN @BidderLeagueTeamId < @LeagueTeamId THEN @BidderLeagueTeamId ELSE @LeagueTeamId END;
    SET @SecondLockId = CASE WHEN @BidderLeagueTeamId < @LeagueTeamId THEN @LeagueTeamId
                             WHEN @BidderLeagueTeamId > @LeagueTeamId THEN @BidderLeagueTeamId
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

    -- Only a pending bid on this player's squad row at this club can be answered, and only while the bidder that was locked is
    -- still its bidder; 0 rows means it is none of those, or someone else answered it in between.
    UPDATE l
    SET Status     = @Status,
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    FROM dbo.LeagueTeamPlayerLoans l
        INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.Id = l.LeagueTeamPlayerId
    WHERE l.Id = @Id
      AND l.Status = 1
      AND l.ToLeagueTeamId = @BidderLeagueTeamId
      AND ltp.LeagueTeamId = @LeagueTeamId
      AND ltp.PlayerId = @PlayerId;

    SET @Answered = @@ROWCOUNT;

    IF @Answered = 0
    BEGIN
        SELECT 0;

        RETURN;
    END

    IF @Status = 2
    BEGIN
        -- The deal's only squad row is the loaned player's.
        SELECT @DealLeagueTeamPlayerId = LeagueTeamPlayerId
        FROM dbo.LeagueTeamPlayerLoans
        WHERE Id = @Id;

        -- Status 1 is TransferStatus Pending and 3 is Rejected. The Id <> @Id test belongs to the loans only: a transfer can
        -- carry the same number.
        UPDATE l
        SET Status     = 3,
            ModifiedAt = @ModifiedAt,
            ModifiedBy = @ModifiedBy
        FROM dbo.LeagueTeamPlayerLoans l
        WHERE l.Status = 1
          AND l.Id <> @Id
          AND l.LeagueTeamPlayerId = @DealLeagueTeamPlayerId;

        UPDATE t
        SET Status     = 3,
            ModifiedAt = @ModifiedAt,
            ModifiedBy = @ModifiedBy
        FROM dbo.LeagueTeamPlayerTransfers t
        WHERE t.Status = 1
          AND (t.LeagueTeamPlayerId = @DealLeagueTeamPlayerId
               OR EXISTS (SELECT 1
                          FROM dbo.LeagueTeamPlayerTransferSwaps sw
                          WHERE sw.LeagueTeamPlayerTransferId = t.Id
                            AND sw.LeagueTeamPlayerId = @DealLeagueTeamPlayerId));
    END

    SELECT @Id;
END
