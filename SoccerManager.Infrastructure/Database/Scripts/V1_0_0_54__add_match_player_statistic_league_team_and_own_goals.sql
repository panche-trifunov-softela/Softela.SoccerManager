-- OwnGoals counts the goals a player put into their own net, which count for the opponent. Goals stays the player's own goals, extra time and in-play penalties included, and never own goals or shootout kicks (those live in dbo.PenaltiesAfterExtraTime).
-- OwnGoals gets a named default of 0, the convention for every column added to an existing table (DF_Matches_Attendance in V1_0_0_15, DF_Matches_IsStarted in V1_0_0_18).
-- LeagueTeamId is the club the player played for in that match. Squads hold only a player's current club, so nothing else records it. The table was empty when this was written (the developer confirmed it), so the column is added NOT NULL with no default: on a table with rows the ADD fails and the script rolls back rather than guess a club, as V1_0_0_27 did for Standings.
-- FK_MatchPlayerStatistics_LeagueTeams does not cascade, as in V1_0_0_41: Leagues already cascade into this table through Seasons and Matches, and a second path through LeagueTeams would be rejected. A league delete still removes the rows through Seasons and Matches, and dbo.DeleteLeagueTeam checks for them before a roster delete.
-- IX_MatchPlayerStatistics_LeagueTeamId backs that foreign key and the roster check.
-- CK_MatchPlayerStatistics_CountersNonNegative is dropped and re-added so one check still covers every counter, now eight. V1_0_0_14 itself is never edited: Evolve rejects a changed checksum.
-- GO is required before the re-added CHECK: SQL Server compiles a whole batch before running it, so a CHECK cannot name a column the same batch adds (V1_0_0_15). Evolve still runs both batches in one transaction.
ALTER TABLE dbo.MatchPlayerStatistics ADD OwnGoals INT NOT NULL CONSTRAINT DF_MatchPlayerStatistics_OwnGoals DEFAULT 0;

ALTER TABLE dbo.MatchPlayerStatistics ADD LeagueTeamId INT NOT NULL;
GO

ALTER TABLE dbo.MatchPlayerStatistics DROP CONSTRAINT CK_MatchPlayerStatistics_CountersNonNegative;

ALTER TABLE dbo.MatchPlayerStatistics ADD CONSTRAINT CK_MatchPlayerStatistics_CountersNonNegative CHECK (MinutesPlayed >= 0 AND Goals >= 0 AND OwnGoals >= 0 AND Assists >= 0 AND PenaltiesScored >= 0 AND PenaltiesMissed >= 0 AND YellowCards >= 0 AND RedCards >= 0);

CREATE NONCLUSTERED INDEX IX_MatchPlayerStatistics_LeagueTeamId ON dbo.MatchPlayerStatistics (LeagueTeamId);

ALTER TABLE dbo.MatchPlayerStatistics ADD CONSTRAINT FK_MatchPlayerStatistics_LeagueTeams FOREIGN KEY (LeagueTeamId) REFERENCES dbo.LeagueTeams (Id);
