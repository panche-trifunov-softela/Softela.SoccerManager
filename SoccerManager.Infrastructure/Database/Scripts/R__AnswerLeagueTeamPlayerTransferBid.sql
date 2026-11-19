-- Answers one pending transfer bid on a player of @LeagueTeamId, behind PUT /api/league-teams/{id}/league-players/{playerId}/
-- transfer-bids/{bidId}. @Status is a TransferStatus value: 2 Accepted or 3 Rejected (1 is Pending). It gives the bid's Id
-- when the answer was recorded and 0 when nothing was answered: no such bid, a bid that is no longer pending, a bid on another
-- player's squad row or at another club, a bid whose seller (FromLeagueTeamId) is not @LeagueTeamId, or an accept while a
-- player of the deal already has an agreed deal. It does not check access: the command handler does that first, through
-- IClubAccess.
-- An accepted bid only records the agreement: nobody moves and no squad row, lineup or other table changes. A later Generator
-- step completes accepted deals. Accepting does reject (Status 3) every OTHER pending bid that involves a player of the deal:
-- the squad row of the bid and the squad rows swapped in it. Those are the pending transfers on one of those rows, the pending
-- transfers that offer one of them in a swap, and the pending loans on one of them. That covers the bids pending at that
-- moment only; a bid placed later is refused when it is placed, because a player whose deal is agreed takes no new bid
-- (dbo.GetLeagueTeamPlayerHasAgreedDeal). For the same reason an accept is refused while a player of the deal, the sold one or
-- a swapped one, already has an agreed deal (a transfer with Status 2 on that squad row, a swap row of a transfer with Status 2
-- naming it, or a loan with Status 2 on it): the check runs after the two locks and before the answer is written, so nothing
-- changes and 0 is returned. A pending bid like that can no longer arise, and a migration rejected the ones that existed. The
-- answered bid is left out of its own table's rejection; it is no longer pending anyway.
-- It runs inside the caller's transaction and first locks the LeagueTeams rows of both clubs of the deal, the selling club
-- (@LeagueTeamId) and the bidding club (the bid's ToLeagueTeamId, read before the lock; NULL when there is no such bid), each
-- WITH (UPDLOCK, HOLDLOCK), as dbo.GetLeagueTeamHasPendingBid does for a bidder. Every squad row of a deal belongs to its seller
-- or to its bidder (a swapped player belongs to the bidder), so two answers that could touch each other's bids always share one
-- of the two clubs, and holding both locks makes them run one after the other: the second sees the first one's result and gets
-- 0 or finds the other bid already rejected, instead of both succeeding or waiting on each other's row locks. The two rows are
-- locked in ascending Id order, one single-row statement each, so two answers never wait on each other in opposite orders.
-- LeagueTeams is locked first, as dbo.DeleteLeagueTeam and dbo.GetLeagueTeamHasPendingBid do, so those never wait on this in
-- opposite orders either.
CREATE OR ALTER PROCEDURE dbo.AnswerLeagueTeamPlayerTransferBid
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
    DECLARE @BidderLeagueTeamId INT;
    DECLARE @FirstLockId INT;
    DECLARE @SecondLockId INT;

    SELECT @BidderLeagueTeamId = ToLeagueTeamId
    FROM dbo.LeagueTeamPlayerTransfers
    WHERE Id = @Id;

    -- The lower Id is locked first; the second lock is skipped when there is no bidder or the bidder is the selling club.
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

    DECLARE @DealRows TABLE (Id INT NOT NULL PRIMARY KEY);

    -- A second accept is refused: no squad row of the deal may already be in an agreed deal. Status 2 is TransferStatus Accepted.
    -- The squad rows of the deal are the sold player's and the swapped players'.
    IF @Status = 2
    BEGIN
        INSERT INTO @DealRows (Id)
        SELECT t.LeagueTeamPlayerId
        FROM dbo.LeagueTeamPlayerTransfers t
        WHERE t.Id = @Id
        UNION
        SELECT sw.LeagueTeamPlayerId
        FROM dbo.LeagueTeamPlayerTransferSwaps sw
        WHERE sw.LeagueTeamPlayerTransferId = @Id;

        IF EXISTS (SELECT 1
                   FROM dbo.LeagueTeamPlayerTransfers a
                   WHERE a.Status = 2
                     AND a.LeagueTeamPlayerId IN (SELECT Id FROM @DealRows))
           OR EXISTS (SELECT 1
                      FROM dbo.LeagueTeamPlayerTransferSwaps asw
                          INNER JOIN dbo.LeagueTeamPlayerTransfers a ON a.Id = asw.LeagueTeamPlayerTransferId
                      WHERE a.Status = 2
                        AND asw.LeagueTeamPlayerId IN (SELECT Id FROM @DealRows))
           OR EXISTS (SELECT 1
                      FROM dbo.LeagueTeamPlayerLoans al
                      WHERE al.Status = 2
                        AND al.LeagueTeamPlayerId IN (SELECT Id FROM @DealRows))
        BEGIN
            SELECT 0;

            RETURN;
        END
    END

    -- Only a pending bid of this club on this player's squad row can be answered, and only while the bidder that was locked is
    -- still its bidder; 0 rows means it is none of those, or someone else answered it in between.
    UPDATE t
    SET Status     = @Status,
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    FROM dbo.LeagueTeamPlayerTransfers t
        INNER JOIN dbo.LeagueTeamPlayers ltp ON ltp.Id = t.LeagueTeamPlayerId
    WHERE t.Id = @Id
      AND t.Status = 1
      AND t.FromLeagueTeamId = @LeagueTeamId
      AND t.ToLeagueTeamId = @BidderLeagueTeamId
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
        -- @DealRows, the squad rows of the agreed deal, was filled before the answer was written. Status 1 is TransferStatus
        -- Pending and 3 is Rejected. The Id <> @Id test belongs to the transfers only: a loan can carry the same number.
        UPDATE t
        SET Status     = 3,
            ModifiedAt = @ModifiedAt,
            ModifiedBy = @ModifiedBy
        FROM dbo.LeagueTeamPlayerTransfers t
        WHERE t.Status = 1
          AND t.Id <> @Id
          AND (t.LeagueTeamPlayerId IN (SELECT Id FROM @DealRows)
               OR EXISTS (SELECT 1
                          FROM dbo.LeagueTeamPlayerTransferSwaps sw
                          WHERE sw.LeagueTeamPlayerTransferId = t.Id
                            AND sw.LeagueTeamPlayerId IN (SELECT Id FROM @DealRows)));

        UPDATE l
        SET Status     = 3,
            ModifiedAt = @ModifiedAt,
            ModifiedBy = @ModifiedBy
        FROM dbo.LeagueTeamPlayerLoans l
        WHERE l.Status = 1
          AND l.LeagueTeamPlayerId IN (SELECT Id FROM @DealRows);
    END

    SELECT @Id;
END
