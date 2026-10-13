-- Manager appointments (dbo.LeagueTeamManagers) now hang off the league roster: LeagueTeamId replaces the LeagueId/TeamId pair, so a club can only be managed in a league whose roster it is on.
-- The roster is topped up first with any appointment pair not on it yet (stamped like the V1_0_0_36 backfill), so every appointment finds its LeagueTeams row below.
-- FK_LeagueTeamManagers_LeagueTeams cascades: with the direct Leagues and Teams foreign keys gone, LeagueTeams is the only path from either parent, so deleting a league, a team or a roster row removes its appointments. FK_LeagueTeamManagers_Managers is unchanged.
-- UQ_LeagueTeamManagers_LeagueTeamId_ManagerId_StartDate and UX_LeagueTeamManagers_LeagueTeamId_Current replace their (LeagueId, TeamId) versions one for one: a club still has at most one current manager per league, and the key leads on LeagueTeamId, so it also backs the cascade and the GetLeagueTeamManagers lookup.
DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();
DECLARE @ImportUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';

INSERT INTO dbo.LeagueTeams (LeagueId, TeamId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
SELECT pairs.LeagueId, pairs.TeamId, @Now, @Now, @ImportUserId, @ImportUserId
FROM (SELECT DISTINCT LeagueId, TeamId FROM dbo.LeagueTeamManagers) AS pairs
WHERE NOT EXISTS (SELECT 1 FROM dbo.LeagueTeams lt WHERE lt.LeagueId = pairs.LeagueId AND lt.TeamId = pairs.TeamId);

ALTER TABLE dbo.LeagueTeamManagers ADD LeagueTeamId INT NULL;
GO

UPDATE ltm
SET LeagueTeamId = lt.Id
FROM dbo.LeagueTeamManagers ltm
    INNER JOIN dbo.LeagueTeams lt ON lt.LeagueId = ltm.LeagueId AND lt.TeamId = ltm.TeamId;
GO

ALTER TABLE dbo.LeagueTeamManagers ALTER COLUMN LeagueTeamId INT NOT NULL;

ALTER TABLE dbo.LeagueTeamManagers DROP CONSTRAINT FK_LeagueTeamManagers_Leagues;

ALTER TABLE dbo.LeagueTeamManagers DROP CONSTRAINT FK_LeagueTeamManagers_Teams;

ALTER TABLE dbo.LeagueTeamManagers DROP CONSTRAINT UQ_LeagueTeamManagers_LeagueId_TeamId_ManagerId_StartDate;

DROP INDEX UX_LeagueTeamManagers_LeagueId_TeamId_Current ON dbo.LeagueTeamManagers;

ALTER TABLE dbo.LeagueTeamManagers DROP COLUMN LeagueId, TeamId;

ALTER TABLE dbo.LeagueTeamManagers ADD CONSTRAINT UQ_LeagueTeamManagers_LeagueTeamId_ManagerId_StartDate UNIQUE (LeagueTeamId, ManagerId, StartDate);

CREATE UNIQUE NONCLUSTERED INDEX UX_LeagueTeamManagers_LeagueTeamId_Current
    ON dbo.LeagueTeamManagers (LeagueTeamId)
    WHERE IsCurrent = 1;

ALTER TABLE dbo.LeagueTeamManagers ADD CONSTRAINT FK_LeagueTeamManagers_LeagueTeams FOREIGN KEY (LeagueTeamId) REFERENCES dbo.LeagueTeams (Id) ON DELETE CASCADE;
