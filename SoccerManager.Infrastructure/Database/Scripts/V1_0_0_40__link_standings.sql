-- Standings now reference the league roster: LeagueTeamId replaces TeamId, so a standings row can only exist for a club on the roster of its league.
-- A row reaches its league through both its season and its competition, and nothing in the schema ties the two together, so the script first stops with an error, changing nothing, if any row's season and competition are in different leagues; such rows must be fixed or deleted by hand before it can run.
-- The roster is then topped up with any standings pair not on it yet (the league taken from dbo.Competitions, as in the V1_0_0_36 backfill, and stamped like it), so every row finds its LeagueTeams row below.
-- FK_Standings_LeagueTeams does not cascade: Leagues already cascade into Standings through Seasons, and a second path through LeagueTeams would be rejected. A league delete still removes its standings through Seasons, while a team or roster entry with standings cannot be deleted, as with FK_Standings_Teams before.
-- UQ_Standings_CompetitionId_SeasonId_LeagueTeamId replaces the (CompetitionId, SeasonId, TeamId) key one for one, and IX_Standings_LeagueTeamId backs the foreign key and the roster-removal check, which look standings up by LeagueTeamId alone.
DECLARE @Mismatched INT = (
    SELECT COUNT(*)
    FROM dbo.Standings s
        INNER JOIN dbo.Seasons se ON se.Id = s.SeasonId
        INNER JOIN dbo.Competitions c ON c.Id = s.CompetitionId
    WHERE se.LeagueId <> c.LeagueId);

IF @Mismatched > 0
BEGIN
    DECLARE @Message NVARCHAR(2048) = CONCAT(N'V1_0_0_40: ', @Mismatched, N' standings row(s) have a season and a competition in different leagues. Fix or delete them, then start the API again.');

    THROW 50000, @Message, 1;
END
GO

DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();
DECLARE @ImportUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';

INSERT INTO dbo.LeagueTeams (LeagueId, TeamId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
SELECT pairs.LeagueId, pairs.TeamId, @Now, @Now, @ImportUserId, @ImportUserId
FROM (
    SELECT DISTINCT c.LeagueId, s.TeamId
    FROM dbo.Standings s
        INNER JOIN dbo.Competitions c ON c.Id = s.CompetitionId
) AS pairs
WHERE NOT EXISTS (SELECT 1 FROM dbo.LeagueTeams lt WHERE lt.LeagueId = pairs.LeagueId AND lt.TeamId = pairs.TeamId);

ALTER TABLE dbo.Standings ADD LeagueTeamId INT NULL;
GO

UPDATE s
SET LeagueTeamId = lt.Id
FROM dbo.Standings s
    INNER JOIN dbo.Competitions c ON c.Id = s.CompetitionId
    INNER JOIN dbo.LeagueTeams lt ON lt.LeagueId = c.LeagueId AND lt.TeamId = s.TeamId;
GO

ALTER TABLE dbo.Standings ALTER COLUMN LeagueTeamId INT NOT NULL;

ALTER TABLE dbo.Standings DROP CONSTRAINT FK_Standings_Teams;

ALTER TABLE dbo.Standings DROP CONSTRAINT UQ_Standings_CompetitionId_SeasonId_TeamId;

ALTER TABLE dbo.Standings DROP COLUMN TeamId;

ALTER TABLE dbo.Standings ADD CONSTRAINT UQ_Standings_CompetitionId_SeasonId_LeagueTeamId UNIQUE (CompetitionId, SeasonId, LeagueTeamId);

CREATE NONCLUSTERED INDEX IX_Standings_LeagueTeamId ON dbo.Standings (LeagueTeamId);

ALTER TABLE dbo.Standings ADD CONSTRAINT FK_Standings_LeagueTeams FOREIGN KEY (LeagueTeamId) REFERENCES dbo.LeagueTeams (Id);
