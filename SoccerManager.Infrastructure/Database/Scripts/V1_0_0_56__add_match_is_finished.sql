-- V1_0_0_13 is already applied and is never edited; Evolve rejects a changed checksum.
-- IsFinished is NOT NULL with a named default of 0 because ALTER TABLE ... ADD of a NOT NULL column fails on a table that already has rows; 0 (not finished) is also the right value for every match that already exists, and for every match inserted later, since dbo.InsertMatch does not set it.
-- CK_Matches_FinishedHasStarted mirrors UpdateMatchValidator's rule that a finished match has started, so the two agree. Every existing row passes it, since all of them read IsFinished = 0.
-- GO is required: SQL Server compiles a whole batch before running it, so the CHECK cannot name a column the same batch adds (V1_0_0_15). Evolve still runs both batches in one transaction.
ALTER TABLE dbo.Matches ADD IsFinished BIT NOT NULL CONSTRAINT DF_Matches_IsFinished DEFAULT 0;
GO

ALTER TABLE dbo.Matches ADD CONSTRAINT CK_Matches_FinishedHasStarted CHECK (IsFinished = 0 OR IsStarted = 1);
