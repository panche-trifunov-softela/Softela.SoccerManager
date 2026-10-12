-- The roster of a league (gameworld): one row per club taking part in it. Until now membership was only implied by squads (dbo.LeagueTeamPlayers), per-season standings and manager appointments.
-- Both foreign keys cascade: Leagues and Teams are independent cascade roots and nothing references this table yet, so each parent reaches it by exactly one path, as for dbo.LeagueTeamManagers.
-- UQ_LeagueTeams_LeagueId_TeamId keeps a club on a league's roster at most once; it leads on LeagueId, so it also backs the Leagues cascade and the GetLeagueTeams lookup. IX_LeagueTeams_TeamId backs the Teams cascade.
-- The backfill adds every (LeagueId, TeamId) pair already present in squads, standings (the league taken from dbo.Competitions, the foreign-key path, not dbo.Seasons) and manager appointments, stamped with the importer's fixed user and the migration time. A fresh database has nothing to backfill.
CREATE TABLE dbo.LeagueTeams (
    Id         INT              IDENTITY(1,1)    NOT NULL,
    LeagueId   INT                               NOT NULL,
    TeamId     INT                               NOT NULL,
    CreatedAt  DATETIME2(7)                      NOT NULL,
    ModifiedAt DATETIME2(7)                      NOT NULL,
    CreatedBy  UNIQUEIDENTIFIER                  NOT NULL,
    ModifiedBy UNIQUEIDENTIFIER                  NOT NULL,
    CONSTRAINT PK_LeagueTeams PRIMARY KEY CLUSTERED (Id),
    CONSTRAINT FK_LeagueTeams_Leagues FOREIGN KEY (LeagueId) REFERENCES dbo.Leagues (Id) ON DELETE CASCADE,
    CONSTRAINT FK_LeagueTeams_Teams FOREIGN KEY (TeamId) REFERENCES dbo.Teams (Id) ON DELETE CASCADE,
    CONSTRAINT UQ_LeagueTeams_LeagueId_TeamId UNIQUE (LeagueId, TeamId)
);

CREATE NONCLUSTERED INDEX IX_LeagueTeams_TeamId ON dbo.LeagueTeams (TeamId);
GO

DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();
DECLARE @ImportUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';

INSERT INTO dbo.LeagueTeams (LeagueId, TeamId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
SELECT pairs.LeagueId, pairs.TeamId, @Now, @Now, @ImportUserId, @ImportUserId
FROM (
    SELECT LeagueId, TeamId FROM dbo.LeagueTeamPlayers
    UNION
    SELECT c.LeagueId, s.TeamId FROM dbo.Standings s INNER JOIN dbo.Competitions c ON c.Id = s.CompetitionId
    UNION
    SELECT LeagueId, TeamId FROM dbo.LeagueTeamManagers
) AS pairs;
