-- V1_0_0_03, V1_0_0_08 and V1_0_0_13 are already applied and are never edited; Evolve rejects a changed checksum.
-- A Competition now is what a Division was, plus cups, so the four Division columns move onto dbo.Competitions and dbo.Divisions is dropped outright.
-- Each added column is NOT NULL with a named default because ALTER TABLE ... ADD of a NOT NULL column fails on a table that already has rows; 0 is the right value for every existing competition, since a cup has no tier, promotes nobody and relegates nobody.
-- UX_Competitions_LeagueId_Order is FILTERED to [Order] > 0 rather than a plain UNIQUE constraint, because every cup shares Order 0 and a plain UNIQUE (LeagueId, [Order]) would therefore permit only one cup per league, as SQL Server treats repeated values, and repeated NULLs, as collisions. The filter keeps tiers unique per league while leaving cups unconstrained. UX_LeagueTeamManagers_LeagueId_TeamId_Current, from V1_0_0_09, is the precedent for a filtered unique index here.
-- Matches.CompetitionId is NULLABLE, unlike the NOT NULL DivisionId it replaces, because a friendly match belongs to no competition. Nullability also means the column can be added to a populated table with no default and no backfill, and the foreign key's WITH CHECK validation passes on every existing row, since a NULL is exempt from foreign-key validation.
-- FK_Matches_Competitions omits ON DELETE, so it is NO ACTION. SET NULL would be tempting for a nullable column, but SQL Server counts SET NULL as a cascade path, and Leagues -> Seasons -> Matches already cascades while Competitions also hangs off Leagues, so it would give Leagues two paths into Matches and be rejected, the same reason FK_Matches_Divisions was NO ACTION.
-- IX_Matches_CompetitionId replaces the dropped IX_Matches_DivisionId and backs the NO ACTION check SQL Server runs whenever a competition is deleted.
-- On Standings, DivisionId was a non-leading column of UQ_Standings_CompetitionId_SeasonId_DivisionId_TeamId, so the replacement constraint simply drops it; one team still has one row per competition per season, which is now the whole of what a standings row is keyed by. The constraint still leads on CompetitionId, so it keeps backing FK_Standings_Competitions and the WHERE CompetitionId = @CompetitionId lookup in GetStandings.
-- Every constraint and index referencing a column is dropped before the column itself, and every foreign key pointing at dbo.Divisions is dropped before the table; SQL Server refuses otherwise, the same rule V1_0_0_16 followed for the Match team columns. PK_Divisions, FK_Divisions_Leagues and UQ_Divisions_LeagueId_Order need no explicit drop, since they belong to the table being removed.
-- The five Division stored procedures are dropped with IF EXISTS, not a bare DROP PROCEDURE, because Evolve runs every versioned (V*) script before any repeatable (R__) script, so on a brand-new database this migration runs at a point where the Division procedures have never been created; their R__ files are deleted in this same change, so a bare DROP PROCEDURE would fail there and block startup.

ALTER TABLE dbo.Competitions ADD [Order] INT NOT NULL CONSTRAINT DF_Competitions_Order DEFAULT 0;

ALTER TABLE dbo.Competitions ADD TeamsPromoted INT NOT NULL CONSTRAINT DF_Competitions_TeamsPromoted DEFAULT 0;

ALTER TABLE dbo.Competitions ADD TeamsRelegated INT NOT NULL CONSTRAINT DF_Competitions_TeamsRelegated DEFAULT 0;

ALTER TABLE dbo.Competitions ADD TeamsInPlayoffs INT NOT NULL CONSTRAINT DF_Competitions_TeamsInPlayoffs DEFAULT 0;
GO

CREATE UNIQUE NONCLUSTERED INDEX UX_Competitions_LeagueId_Order ON dbo.Competitions (LeagueId, [Order]) WHERE [Order] > 0;

ALTER TABLE dbo.Matches ADD CompetitionId INT NULL;

ALTER TABLE dbo.Matches ADD CONSTRAINT FK_Matches_Competitions FOREIGN KEY (CompetitionId) REFERENCES dbo.Competitions (Id);

CREATE NONCLUSTERED INDEX IX_Matches_CompetitionId ON dbo.Matches (CompetitionId);

ALTER TABLE dbo.Matches DROP CONSTRAINT FK_Matches_Divisions;
DROP INDEX IX_Matches_DivisionId ON dbo.Matches;

ALTER TABLE dbo.Matches DROP COLUMN DivisionId;

ALTER TABLE dbo.Standings DROP CONSTRAINT FK_Standings_Divisions;
ALTER TABLE dbo.Standings DROP CONSTRAINT UQ_Standings_CompetitionId_SeasonId_DivisionId_TeamId;

ALTER TABLE dbo.Standings ADD CONSTRAINT UQ_Standings_CompetitionId_SeasonId_TeamId UNIQUE (CompetitionId, SeasonId, TeamId);

ALTER TABLE dbo.Standings DROP COLUMN DivisionId;

DROP PROCEDURE IF EXISTS dbo.InsertDivision;
DROP PROCEDURE IF EXISTS dbo.UpdateDivision;
DROP PROCEDURE IF EXISTS dbo.DeleteDivision;
DROP PROCEDURE IF EXISTS dbo.GetDivisionById;
DROP PROCEDURE IF EXISTS dbo.GetDivisions;

DROP TABLE dbo.Divisions;
