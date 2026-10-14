-- Match team statistics now reference the league roster: LeagueTeamId replaces TeamId, so a statistics row can only exist for a club on the roster of its match's league.
-- A row reaches its league only through its match's season, and both links are NOT NULL, so unlike V1_0_0_40 there is no second path that could disagree and no pre-check.
-- The roster is topped up first with any statistics pair not on it yet (the league taken from the match's season, and stamped like the V1_0_0_36 backfill), so every row finds its LeagueTeams row below.
-- FK_MatchTeamStatistics_LeagueTeams does not cascade: Leagues already cascade into MatchTeamStatistics through Seasons and Matches, and a second path through LeagueTeams would be rejected. A league delete still removes its statistics through Seasons and Matches, while a team or roster entry with statistics can no longer be deleted (FK_MatchTeamStatistics_Teams used to cascade them away).
-- UQ_MatchTeamStatistics_MatchId_LeagueTeamId replaces the (MatchId, TeamId) key one for one and keeps MatchId leading for the per-match lookups, and IX_MatchTeamStatistics_LeagueTeamId replaces IX_MatchTeamStatistics_TeamId, backing the foreign key and the roster-removal check.
DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();
DECLARE @ImportUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';

INSERT INTO dbo.LeagueTeams (LeagueId, TeamId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
SELECT pairs.LeagueId, pairs.TeamId, @Now, @Now, @ImportUserId, @ImportUserId
FROM (
    SELECT DISTINCT se.LeagueId, mts.TeamId
    FROM dbo.MatchTeamStatistics mts
        INNER JOIN dbo.Matches m ON m.Id = mts.MatchId
        INNER JOIN dbo.Seasons se ON se.Id = m.SeasonId
) AS pairs
WHERE NOT EXISTS (SELECT 1 FROM dbo.LeagueTeams lt WHERE lt.LeagueId = pairs.LeagueId AND lt.TeamId = pairs.TeamId);

ALTER TABLE dbo.MatchTeamStatistics ADD LeagueTeamId INT NULL;
GO

UPDATE mts
SET LeagueTeamId = lt.Id
FROM dbo.MatchTeamStatistics mts
    INNER JOIN dbo.Matches m ON m.Id = mts.MatchId
    INNER JOIN dbo.Seasons se ON se.Id = m.SeasonId
    INNER JOIN dbo.LeagueTeams lt ON lt.LeagueId = se.LeagueId AND lt.TeamId = mts.TeamId;
GO

ALTER TABLE dbo.MatchTeamStatistics ALTER COLUMN LeagueTeamId INT NOT NULL;

ALTER TABLE dbo.MatchTeamStatistics DROP CONSTRAINT FK_MatchTeamStatistics_Teams;

ALTER TABLE dbo.MatchTeamStatistics DROP CONSTRAINT UQ_MatchTeamStatistics_MatchId_TeamId;

DROP INDEX IX_MatchTeamStatistics_TeamId ON dbo.MatchTeamStatistics;

ALTER TABLE dbo.MatchTeamStatistics DROP COLUMN TeamId;

ALTER TABLE dbo.MatchTeamStatistics ADD CONSTRAINT UQ_MatchTeamStatistics_MatchId_LeagueTeamId UNIQUE (MatchId, LeagueTeamId);

CREATE NONCLUSTERED INDEX IX_MatchTeamStatistics_LeagueTeamId ON dbo.MatchTeamStatistics (LeagueTeamId);

ALTER TABLE dbo.MatchTeamStatistics ADD CONSTRAINT FK_MatchTeamStatistics_LeagueTeams FOREIGN KEY (LeagueTeamId) REFERENCES dbo.LeagueTeams (Id);
