-- V1_0_0_69 and every earlier script are already applied and are never edited; Evolve rejects a changed checksum.
-- TransferStatus (SoccerManager.Domain.Enums) gains Completed, 4: a deal agreed and carried out, so the player has moved. The Generator step that completes accepted deals sets it later, and an operator can set it through the CRUD today. dbo.LeagueTeamPlayerTransfers and dbo.LeagueTeamPlayerLoans share TransferStatus, so both CHECKs widen, from BETWEEN 1 AND 3 to BETWEEN 1 AND 4; a CHECK cannot be altered, so each is dropped and re-added (V1_0_0_69 is the precedent).
-- From now on a player whose deal is agreed (Status 2) takes no new bid and cannot be offered in a swap, and a second accept is refused. Pending bids already waiting on such a player, or offering him in a swap, would stay pending next to the agreed deal for good, so they are rejected here once (Status 3), stamped with the import user as earlier backfills are (V1_0_0_65). Nothing else changes: every other row is left as it is.
-- The agreed squad rows are the player of every accepted transfer, the swapped players of every accepted transfer, and the player of every accepted loan. A pending transfer is rejected when its own player or one of its swapped players is among them, a pending loan when its player is.
-- The CHECKs and the one-off share one batch: no GO is needed, since nothing here names a column the batch adds (V1_0_0_15).
ALTER TABLE dbo.LeagueTeamPlayerTransfers DROP CONSTRAINT CK_LeagueTeamPlayerTransfers_StatusKnown;
ALTER TABLE dbo.LeagueTeamPlayerTransfers ADD CONSTRAINT CK_LeagueTeamPlayerTransfers_StatusKnown CHECK (Status BETWEEN 1 AND 4);

ALTER TABLE dbo.LeagueTeamPlayerLoans DROP CONSTRAINT CK_LeagueTeamPlayerLoans_StatusKnown;
ALTER TABLE dbo.LeagueTeamPlayerLoans ADD CONSTRAINT CK_LeagueTeamPlayerLoans_StatusKnown CHECK (Status BETWEEN 1 AND 4);

DECLARE @ImportUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';
DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();

-- The squad rows that are in an agreed deal.
DECLARE @AgreedRows TABLE (Id INT NOT NULL PRIMARY KEY);

INSERT INTO @AgreedRows (Id)
SELECT t.LeagueTeamPlayerId
FROM dbo.LeagueTeamPlayerTransfers t
WHERE t.Status = 2
UNION
SELECT sw.LeagueTeamPlayerId
FROM dbo.LeagueTeamPlayerTransferSwaps sw
    INNER JOIN dbo.LeagueTeamPlayerTransfers t ON t.Id = sw.LeagueTeamPlayerTransferId
WHERE t.Status = 2
UNION
SELECT l.LeagueTeamPlayerId
FROM dbo.LeagueTeamPlayerLoans l
WHERE l.Status = 2;

-- Status 1 is TransferStatus Pending and 3 is Rejected.
UPDATE t
SET Status     = 3,
    ModifiedAt = @Now,
    ModifiedBy = @ImportUserId
FROM dbo.LeagueTeamPlayerTransfers t
WHERE t.Status = 1
  AND (t.LeagueTeamPlayerId IN (SELECT Id FROM @AgreedRows)
       OR EXISTS (SELECT 1
                  FROM dbo.LeagueTeamPlayerTransferSwaps sw
                  WHERE sw.LeagueTeamPlayerTransferId = t.Id
                    AND sw.LeagueTeamPlayerId IN (SELECT Id FROM @AgreedRows)));

UPDATE l
SET Status     = 3,
    ModifiedAt = @Now,
    ModifiedBy = @ImportUserId
FROM dbo.LeagueTeamPlayerLoans l
WHERE l.Status = 1
  AND l.LeagueTeamPlayerId IN (SELECT Id FROM @AgreedRows);
