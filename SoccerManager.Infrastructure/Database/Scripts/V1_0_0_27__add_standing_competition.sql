-- V1_0_0_08 is already applied and is never edited; Evolve rejects a changed checksum.
-- Existing rows are deleted rather than backfilled: CompetitionId is NOT NULL, and ALTER TABLE ... ADD of a NOT NULL column needs a DEFAULT to backfill existing rows, but every constant default would be an Id that dbo.Competitions does not hold, since Competitions.Id is IDENTITY(1,1) and 0 is never real, so the foreign key's default WITH CHECK validation would reject it. Nothing in a standings row records which competition it belonged to, so no correct value can be derived either. Clearing the table is therefore the only option that leaves the column NOT NULL, and it follows V1_0_0_16, which dropped the Match team columns outright once the developer confirmed the existing rows held nothing worth carrying over. With the table empty the ADD needs no DEFAULT at all.
-- FK_Standings_Competitions omits ON DELETE, so it is NO ACTION: Leagues -> Seasons -> Standings already cascades, and Competitions also hangs off Leagues, so cascading here would give Leagues a second cascade path into Standings, which SQL Server rejects outright, the same reason FK_Standings_Divisions is NO ACTION, as V1_0_0_08 explains.
-- UQ_Standings_SeasonId_DivisionId_TeamId is replaced rather than supplemented, because one team now has one row per competition per season and division, which is what lets a domestic league and a cup group share a division. The new constraint leads on CompetitionId, so it also backs the WHERE CompetitionId = @CompetitionId lookup in GetStandings.
-- IX_Standings_SeasonId is added because the dropped constraint led on SeasonId and was what backed the ON DELETE CASCADE from Seasons; the replacement leads on CompetitionId, demoting SeasonId, so this index restores that backing. DivisionId was already non-leading under the old constraint, so nothing regresses there.
DELETE FROM dbo.Standings;

ALTER TABLE dbo.Standings ADD CompetitionId INT NOT NULL;

ALTER TABLE dbo.Standings ADD CONSTRAINT FK_Standings_Competitions FOREIGN KEY (CompetitionId) REFERENCES dbo.Competitions (Id);

ALTER TABLE dbo.Standings DROP CONSTRAINT UQ_Standings_SeasonId_DivisionId_TeamId;

ALTER TABLE dbo.Standings ADD CONSTRAINT UQ_Standings_CompetitionId_SeasonId_DivisionId_TeamId UNIQUE (CompetitionId, SeasonId, DivisionId, TeamId);

CREATE NONCLUSTERED INDEX IX_Standings_SeasonId ON dbo.Standings (SeasonId);
