-- V1_0_0_04, V1_0_0_05 and V1_0_0_30 are already applied and are never edited; Evolve rejects a changed checksum.
-- TransfermarktId holds the row's identifier on Transfermarkt, so a data import can recognise a row it created earlier instead of inserting a duplicate. It is nullable, since a row created by hand has none, and a nullable column needs no backfill, so existing rows are unaffected.
-- Each UX_*_TransfermarktId is a unique index filtered to non-NULL values, so any number of rows can have no identifier while no two rows share one. A UNIQUE constraint cannot be filtered, hence an index, as with UX_Competitions_LeagueId_Order.
ALTER TABLE dbo.Players ADD TransfermarktId INT NULL;

ALTER TABLE dbo.Teams ADD TransfermarktId INT NULL;

ALTER TABLE dbo.NationalTeams ADD TransfermarktId INT NULL;
GO

CREATE UNIQUE NONCLUSTERED INDEX UX_Players_TransfermarktId ON dbo.Players (TransfermarktId) WHERE TransfermarktId IS NOT NULL;

CREATE UNIQUE NONCLUSTERED INDEX UX_Teams_TransfermarktId ON dbo.Teams (TransfermarktId) WHERE TransfermarktId IS NOT NULL;

CREATE UNIQUE NONCLUSTERED INDEX UX_NationalTeams_TransfermarktId ON dbo.NationalTeams (TransfermarktId) WHERE TransfermarktId IS NOT NULL;
