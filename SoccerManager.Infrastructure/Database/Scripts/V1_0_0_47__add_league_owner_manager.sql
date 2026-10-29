-- V1_0_0_01 is already applied and is never edited; Evolve rejects a changed checksum.
-- OwnerManagerId is the manager who owns the league: CreateLeague sets it to the creating user's manager profile, and UpdateLeague never changes it. It is nullable: a league created before this has no owner, and a nullable column needs no backfill, so existing rows are unaffected.
-- FK_Leagues_Managers uses ON DELETE SET NULL, like FK_Players_Teams, so deleting a manager leaves the leagues they own in place, without an owner. CASCADE would be rejected: Managers already cascades into LeagueTeamManagers and LeagueTeamManagerApplications, and Leagues -> LeagueTeams would give it a second path into both. SET NULL updates Leagues rows rather than deleting them, so the path ends at Leagues, and nothing else reaches Leagues from Managers.
-- IX_Leagues_OwnerManagerId backs that SET NULL, since every manager deletion has to find the leagues it owns, the same as IX_Players_TeamId.
ALTER TABLE dbo.Leagues ADD OwnerManagerId INT NULL;

ALTER TABLE dbo.Leagues ADD CONSTRAINT FK_Leagues_Managers FOREIGN KEY (OwnerManagerId) REFERENCES dbo.Managers (Id) ON DELETE SET NULL;

CREATE NONCLUSTERED INDEX IX_Leagues_OwnerManagerId ON dbo.Leagues (OwnerManagerId);
