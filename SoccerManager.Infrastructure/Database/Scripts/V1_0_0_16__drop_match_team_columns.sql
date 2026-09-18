-- The home/away pairing of a match moves to dbo.MatchTeamStatistics (IsHomeTeam), which the next migration creates, so Matches no longer carries team columns. V1_0_0_13 is already applied and is never edited.
-- No backfill: the developer confirmed the existing rows hold nothing worth carrying over, so the columns are dropped outright.
-- Every constraint and index that references the columns is dropped first; SQL Server refuses to drop a column while one still references it.
ALTER TABLE dbo.Matches DROP CONSTRAINT CK_Matches_DifferentTeams;
ALTER TABLE dbo.Matches DROP CONSTRAINT FK_Matches_HomeTeams;
ALTER TABLE dbo.Matches DROP CONSTRAINT FK_Matches_AwayTeams;
DROP INDEX IX_Matches_HomeTeamId ON dbo.Matches;
DROP INDEX IX_Matches_AwayTeamId ON dbo.Matches;

ALTER TABLE dbo.Matches DROP COLUMN HomeTeamId, AwayTeamId;
