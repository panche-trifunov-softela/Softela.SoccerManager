-- V1_0_0_05 is already applied and is never edited; Evolve rejects a changed checksum.
-- NationalTeamId is nullable: most players never play for a national team, and a nullable column needs no backfill, so existing rows are unaffected.
-- FK_Players_NationalTeams uses ON DELETE SET NULL, like FK_Teams_Stadiums, so deleting a national team un-assigns its players. SET NULL updates Players rows rather than deleting them, so the path ends there and never reaches the LeagueTeamPlayers cascade; SQL Server accepts it, unlike the V1_0_0_28 case, where SET NULL would have been a second path into Matches itself.
-- IX_Players_NationalTeamId backs that SET NULL, since every national team deletion has to find its players; IX_LeagueTeamPlayers_PlayerId is the precedent for indexing a cascade.
ALTER TABLE dbo.Players ADD NationalTeamId INT NULL;

ALTER TABLE dbo.Players ADD CONSTRAINT FK_Players_NationalTeams FOREIGN KEY (NationalTeamId) REFERENCES dbo.NationalTeams (Id) ON DELETE SET NULL;

CREATE NONCLUSTERED INDEX IX_Players_NationalTeamId ON dbo.Players (NationalTeamId);
