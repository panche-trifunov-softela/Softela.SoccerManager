-- V1_0_0_26, V1_0_0_36 and V1_0_0_65 are already applied and are never edited; Evolve rejects a changed checksum.
-- A national team joins a League's roster as a dbo.LeagueTeams row with NationalTeamId set and TeamId NULL, so every table keyed by LeagueTeamId (the match tables, appointments, applications) serves a national team as it serves a club. CK_LeagueTeams_TeamOrNationalTeam lets a row have exactly one of the two, and CK_LeagueTeams_NationalTeamNotGuest keeps a national team from being a guest.
-- TeamId becomes nullable. SQL Server does not alter a column that a key or an index depends on, so UQ_LeagueTeams_LeagueId_TeamId, IX_LeagueTeams_TeamId and FK_LeagueTeams_Teams are dropped first and re-created after, the foreign key with its cascade unchanged.
-- A plain UNIQUE treats NULLs as equal and would allow one national team per League, so the club key returns as UX_LeagueTeams_LeagueId_TeamId filtered to TeamId IS NOT NULL, with UX_LeagueTeams_LeagueId_NationalTeamId beside it (filtered unique indexes, as UX_Competitions_LeagueId_Order in V1_0_0_28). Neither covers every row, so IX_LeagueTeams_LeagueId now backs the Leagues cascade and the roster reads that the dropped key backed.
-- FK_LeagueTeams_NationalTeams cascades, as FK_LeagueTeams_Teams does: NationalTeams reaches dbo.Players only through SET NULL (V1_0_0_31), the same shape Teams has (V1_0_0_32), so each parent reaches this table by one path. IX_LeagueTeams_NationalTeamId backs that cascade.
-- Competitions.IsNational marks a competition for national teams, a League's World Cup, so the Generator never takes it for a club cup; CK_Competitions_NationalIsCup keeps it a cup (Order 0) that is not domestic. Every existing competition is a club competition and takes the default 0.
-- Nothing is backfilled: no League has a national competition yet, and a League's national teams are put on its roster by dbo.InsertNationalLeagueTeams when its national competition is created.
-- GO is required: SQL Server compiles a whole batch before running it, so the keys cannot name columns the same batch adds (V1_0_0_15). Evolve still runs all batches in one transaction.
ALTER TABLE dbo.LeagueTeams DROP CONSTRAINT UQ_LeagueTeams_LeagueId_TeamId;
DROP INDEX IX_LeagueTeams_TeamId ON dbo.LeagueTeams;
ALTER TABLE dbo.LeagueTeams DROP CONSTRAINT FK_LeagueTeams_Teams;
ALTER TABLE dbo.LeagueTeams ALTER COLUMN TeamId INT NULL;
ALTER TABLE dbo.LeagueTeams ADD NationalTeamId INT NULL;
ALTER TABLE dbo.Competitions ADD IsNational BIT NOT NULL CONSTRAINT DF_Competitions_IsNational DEFAULT 0;
GO

ALTER TABLE dbo.LeagueTeams ADD CONSTRAINT FK_LeagueTeams_Teams FOREIGN KEY (TeamId) REFERENCES dbo.Teams (Id) ON DELETE CASCADE;
ALTER TABLE dbo.LeagueTeams ADD CONSTRAINT FK_LeagueTeams_NationalTeams FOREIGN KEY (NationalTeamId) REFERENCES dbo.NationalTeams (Id) ON DELETE CASCADE;
ALTER TABLE dbo.LeagueTeams ADD CONSTRAINT CK_LeagueTeams_TeamOrNationalTeam CHECK ((TeamId IS NOT NULL AND NationalTeamId IS NULL) OR (TeamId IS NULL AND NationalTeamId IS NOT NULL));
ALTER TABLE dbo.LeagueTeams ADD CONSTRAINT CK_LeagueTeams_NationalTeamNotGuest CHECK (NationalTeamId IS NULL OR IsGuest = 0);
ALTER TABLE dbo.Competitions ADD CONSTRAINT CK_Competitions_NationalIsCup CHECK (IsNational = 0 OR ([Order] = 0 AND IsDomestic = 0));

CREATE UNIQUE NONCLUSTERED INDEX UX_LeagueTeams_LeagueId_TeamId ON dbo.LeagueTeams (LeagueId, TeamId) WHERE TeamId IS NOT NULL;
CREATE UNIQUE NONCLUSTERED INDEX UX_LeagueTeams_LeagueId_NationalTeamId ON dbo.LeagueTeams (LeagueId, NationalTeamId) WHERE NationalTeamId IS NOT NULL;
CREATE NONCLUSTERED INDEX IX_LeagueTeams_LeagueId ON dbo.LeagueTeams (LeagueId);
CREATE NONCLUSTERED INDEX IX_LeagueTeams_TeamId ON dbo.LeagueTeams (TeamId);
CREATE NONCLUSTERED INDEX IX_LeagueTeams_NationalTeamId ON dbo.LeagueTeams (NationalTeamId);
