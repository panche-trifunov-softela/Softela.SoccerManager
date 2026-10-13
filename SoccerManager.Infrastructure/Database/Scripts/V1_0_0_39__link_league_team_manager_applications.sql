-- Manager applications (dbo.LeagueTeamManagerApplications) now hang off the league roster: LeagueTeamId replaces the LeagueId/TeamId pair, so an application can only exist for a club on a league's roster.
-- The roster is topped up first with any application pair not on it yet, whatever the application's status (stamped like the V1_0_0_36 backfill), so every application finds its LeagueTeams row below.
-- FK_LeagueTeamManagerApplications_LeagueTeams cascades: with the direct Leagues and Teams foreign keys gone, LeagueTeams is the only path from either parent, so deleting a league, a team or a roster row removes its applications. FK_LeagueTeamManagerApplications_Managers is unchanged.
-- UX_LeagueTeamManagerApplications_LeagueTeamId_ManagerId_Pending replaces its (LeagueId, TeamId, ManagerId) version one for one, so a manager still has at most one pending application per club.
-- IX_LeagueTeamManagerApplications_LeagueTeamId replaces the (LeagueId, TeamId) and TeamId indexes: the unique index covers pending rows only, so this one backs the cascade and the GetLeagueTeamManagerApplications lookup. IX_LeagueTeamManagerApplications_ManagerId is unchanged.
DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();
DECLARE @ImportUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';

INSERT INTO dbo.LeagueTeams (LeagueId, TeamId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
SELECT pairs.LeagueId, pairs.TeamId, @Now, @Now, @ImportUserId, @ImportUserId
FROM (SELECT DISTINCT LeagueId, TeamId FROM dbo.LeagueTeamManagerApplications) AS pairs
WHERE NOT EXISTS (SELECT 1 FROM dbo.LeagueTeams lt WHERE lt.LeagueId = pairs.LeagueId AND lt.TeamId = pairs.TeamId);

ALTER TABLE dbo.LeagueTeamManagerApplications ADD LeagueTeamId INT NULL;
GO

UPDATE ltma
SET LeagueTeamId = lt.Id
FROM dbo.LeagueTeamManagerApplications ltma
    INNER JOIN dbo.LeagueTeams lt ON lt.LeagueId = ltma.LeagueId AND lt.TeamId = ltma.TeamId;
GO

ALTER TABLE dbo.LeagueTeamManagerApplications ALTER COLUMN LeagueTeamId INT NOT NULL;

ALTER TABLE dbo.LeagueTeamManagerApplications DROP CONSTRAINT FK_LeagueTeamManagerApplications_Leagues;

ALTER TABLE dbo.LeagueTeamManagerApplications DROP CONSTRAINT FK_LeagueTeamManagerApplications_Teams;

DROP INDEX UX_LeagueTeamManagerApplications_LeagueId_TeamId_ManagerId_Pending ON dbo.LeagueTeamManagerApplications;

DROP INDEX IX_LeagueTeamManagerApplications_LeagueId_TeamId ON dbo.LeagueTeamManagerApplications;

DROP INDEX IX_LeagueTeamManagerApplications_TeamId ON dbo.LeagueTeamManagerApplications;

ALTER TABLE dbo.LeagueTeamManagerApplications DROP COLUMN LeagueId, TeamId;

CREATE UNIQUE NONCLUSTERED INDEX UX_LeagueTeamManagerApplications_LeagueTeamId_ManagerId_Pending
    ON dbo.LeagueTeamManagerApplications (LeagueTeamId, ManagerId)
    WHERE Status = 1;

CREATE NONCLUSTERED INDEX IX_LeagueTeamManagerApplications_LeagueTeamId ON dbo.LeagueTeamManagerApplications (LeagueTeamId);

ALTER TABLE dbo.LeagueTeamManagerApplications ADD CONSTRAINT FK_LeagueTeamManagerApplications_LeagueTeams FOREIGN KEY (LeagueTeamId) REFERENCES dbo.LeagueTeams (Id) ON DELETE CASCADE;
