-- V1_0_0_05 is already applied and is never edited; Evolve rejects a changed checksum.
-- TeamId is the player's current real-life club. It is nullable: a free agent has no club, and a nullable column needs no backfill, so existing rows are unaffected.
-- FK_Players_Teams uses ON DELETE SET NULL, like FK_Players_NationalTeams, so deleting a team un-assigns its players. SET NULL ends the cascade at Players, so it never reaches the LeagueTeamPlayers cascade that Teams and Players each already have; Players now has two SET NULL parents, Teams and NationalTeams, but no single deletion reaches Players through both.
-- IX_Players_TeamId backs that SET NULL, since every team deletion has to find its players, the same as IX_Players_NationalTeamId.
ALTER TABLE dbo.Players ADD TeamId INT NULL;

ALTER TABLE dbo.Players ADD CONSTRAINT FK_Players_Teams FOREIGN KEY (TeamId) REFERENCES dbo.Teams (Id) ON DELETE SET NULL;

CREATE NONCLUSTERED INDEX IX_Players_TeamId ON dbo.Players (TeamId);
