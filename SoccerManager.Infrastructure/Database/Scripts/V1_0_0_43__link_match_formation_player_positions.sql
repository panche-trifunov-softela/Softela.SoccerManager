-- Match formation player positions now reference the league roster: LeagueTeamId replaces TeamId, so a lineup slot can only exist for a club on the roster of its match's league.
-- A row reaches its league only through its match's season, and both links are NOT NULL, so as in V1_0_0_41 and V1_0_0_42 there is no second path that could disagree and no pre-check.
-- The roster is topped up first with any lineup pair not on it yet (the league taken from the match's season, and stamped like the V1_0_0_36 backfill), so every row finds its LeagueTeams row below.
-- FK_MatchFormationPlayerPositions_LeagueTeams does not cascade: Leagues already cascade into MatchFormationPlayerPositions through Seasons and Matches, and a second path through LeagueTeams would be rejected. A league delete still removes its lineup slots through Seasons and Matches, while a team or roster entry with lineup slots can no longer be deleted (FK_MatchFormationPlayerPositions_Teams used to cascade them away).
-- UQ_MatchFormationPlayerPositions_MatchId_LeagueTeamId_FormationPositionId replaces the (MatchId, TeamId, FormationPositionId) key one for one and keeps MatchId leading for the per-match lookups and the cascade from Matches, and IX_MatchFormationPlayerPositions_LeagueTeamId replaces IX_MatchFormationPlayerPositions_TeamId, backing the foreign key and the roster-removal check. The formation position and player position foreign keys, their indexes and the snapshot checks are unchanged.
DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();
DECLARE @ImportUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';

INSERT INTO dbo.LeagueTeams (LeagueId, TeamId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
SELECT pairs.LeagueId, pairs.TeamId, @Now, @Now, @ImportUserId, @ImportUserId
FROM (
    SELECT DISTINCT se.LeagueId, mfpp.TeamId
    FROM dbo.MatchFormationPlayerPositions mfpp
        INNER JOIN dbo.Matches m ON m.Id = mfpp.MatchId
        INNER JOIN dbo.Seasons se ON se.Id = m.SeasonId
) AS pairs
WHERE NOT EXISTS (SELECT 1 FROM dbo.LeagueTeams lt WHERE lt.LeagueId = pairs.LeagueId AND lt.TeamId = pairs.TeamId);

ALTER TABLE dbo.MatchFormationPlayerPositions ADD LeagueTeamId INT NULL;
GO

UPDATE mfpp
SET LeagueTeamId = lt.Id
FROM dbo.MatchFormationPlayerPositions mfpp
    INNER JOIN dbo.Matches m ON m.Id = mfpp.MatchId
    INNER JOIN dbo.Seasons se ON se.Id = m.SeasonId
    INNER JOIN dbo.LeagueTeams lt ON lt.LeagueId = se.LeagueId AND lt.TeamId = mfpp.TeamId;
GO

ALTER TABLE dbo.MatchFormationPlayerPositions ALTER COLUMN LeagueTeamId INT NOT NULL;

ALTER TABLE dbo.MatchFormationPlayerPositions DROP CONSTRAINT FK_MatchFormationPlayerPositions_Teams;

ALTER TABLE dbo.MatchFormationPlayerPositions DROP CONSTRAINT UQ_MatchFormationPlayerPositions_MatchId_TeamId_FormationPositionId;

DROP INDEX IX_MatchFormationPlayerPositions_TeamId ON dbo.MatchFormationPlayerPositions;

ALTER TABLE dbo.MatchFormationPlayerPositions DROP COLUMN TeamId;

ALTER TABLE dbo.MatchFormationPlayerPositions ADD CONSTRAINT UQ_MatchFormationPlayerPositions_MatchId_LeagueTeamId_FormationPositionId UNIQUE (MatchId, LeagueTeamId, FormationPositionId);

CREATE NONCLUSTERED INDEX IX_MatchFormationPlayerPositions_LeagueTeamId ON dbo.MatchFormationPlayerPositions (LeagueTeamId);

ALTER TABLE dbo.MatchFormationPlayerPositions ADD CONSTRAINT FK_MatchFormationPlayerPositions_LeagueTeams FOREIGN KEY (LeagueTeamId) REFERENCES dbo.LeagueTeams (Id);
