-- Match team tactics now reference the league roster: LeagueTeamId replaces TeamId, so a tactic can only exist for a club on the roster of its match's league.
-- A row reaches its league only through its match's season, and both links are NOT NULL, so as in V1_0_0_41 there is no second path that could disagree and no pre-check.
-- The roster is topped up first with any tactics pair not on it yet (the league taken from the match's season, and stamped like the V1_0_0_36 backfill), so every row finds its LeagueTeams row below.
-- FK_MatchTeamTactics_LeagueTeams does not cascade: Leagues already cascade into MatchTeamTactics through Seasons and Matches, and a second path through LeagueTeams would be rejected. A league delete still removes its tactics through Seasons and Matches, while a team or roster entry with tactics can no longer be deleted (FK_MatchTeamTactics_Teams used to cascade them away).
-- UQ_MatchTeamTactics_MatchId_LeagueTeamId replaces the (MatchId, TeamId) key one for one and keeps MatchId leading for the per-match lookups, and IX_MatchTeamTactics_LeagueTeamId replaces IX_MatchTeamTactics_TeamId, backing the foreign key and the roster-removal check. The four player foreign keys and their indexes are unchanged.
DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();
DECLARE @ImportUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';

INSERT INTO dbo.LeagueTeams (LeagueId, TeamId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
SELECT pairs.LeagueId, pairs.TeamId, @Now, @Now, @ImportUserId, @ImportUserId
FROM (
    SELECT DISTINCT se.LeagueId, mtt.TeamId
    FROM dbo.MatchTeamTactics mtt
        INNER JOIN dbo.Matches m ON m.Id = mtt.MatchId
        INNER JOIN dbo.Seasons se ON se.Id = m.SeasonId
) AS pairs
WHERE NOT EXISTS (SELECT 1 FROM dbo.LeagueTeams lt WHERE lt.LeagueId = pairs.LeagueId AND lt.TeamId = pairs.TeamId);

ALTER TABLE dbo.MatchTeamTactics ADD LeagueTeamId INT NULL;
GO

UPDATE mtt
SET LeagueTeamId = lt.Id
FROM dbo.MatchTeamTactics mtt
    INNER JOIN dbo.Matches m ON m.Id = mtt.MatchId
    INNER JOIN dbo.Seasons se ON se.Id = m.SeasonId
    INNER JOIN dbo.LeagueTeams lt ON lt.LeagueId = se.LeagueId AND lt.TeamId = mtt.TeamId;
GO

ALTER TABLE dbo.MatchTeamTactics ALTER COLUMN LeagueTeamId INT NOT NULL;

ALTER TABLE dbo.MatchTeamTactics DROP CONSTRAINT FK_MatchTeamTactics_Teams;

ALTER TABLE dbo.MatchTeamTactics DROP CONSTRAINT UQ_MatchTeamTactics_MatchId_TeamId;

DROP INDEX IX_MatchTeamTactics_TeamId ON dbo.MatchTeamTactics;

ALTER TABLE dbo.MatchTeamTactics DROP COLUMN TeamId;

ALTER TABLE dbo.MatchTeamTactics ADD CONSTRAINT UQ_MatchTeamTactics_MatchId_LeagueTeamId UNIQUE (MatchId, LeagueTeamId);

CREATE NONCLUSTERED INDEX IX_MatchTeamTactics_LeagueTeamId ON dbo.MatchTeamTactics (LeagueTeamId);

ALTER TABLE dbo.MatchTeamTactics ADD CONSTRAINT FK_MatchTeamTactics_LeagueTeams FOREIGN KEY (LeagueTeamId) REFERENCES dbo.LeagueTeams (Id);
