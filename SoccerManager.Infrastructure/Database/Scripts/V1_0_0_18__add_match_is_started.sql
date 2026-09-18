-- V1_0_0_13 is already applied and is never edited; Evolve rejects a changed checksum.
-- IsStarted is NOT NULL with a default of 0 because ALTER TABLE ... ADD of a NOT NULL column fails on a table that already has rows; 0 (not started) is also the right value for every match that already exists.
-- No CHECK constraint: BIT admits only 0 and 1, so there is nothing further to constrain.
ALTER TABLE dbo.Matches ADD IsStarted BIT NOT NULL CONSTRAINT DF_Matches_IsStarted DEFAULT 0;
