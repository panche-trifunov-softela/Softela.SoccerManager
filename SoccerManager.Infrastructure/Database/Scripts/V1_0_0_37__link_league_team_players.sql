-- Squads (dbo.LeagueTeamPlayers) now hang off the league roster: LeagueTeamId replaces the LeagueId/TeamId pair, so a squad row can only exist for a club on the league's roster.
-- The roster is topped up first with any squad pair not on it yet (stamped like the V1_0_0_36 backfill), so every squad row finds its LeagueTeams row below.
-- FK_LeagueTeamPlayers_LeagueTeams cascades: with the direct Leagues and Teams foreign keys gone, LeagueTeams is the only path from either parent, so deleting a league, a team or a roster row removes its squad rows.
-- UQ_LeagueTeamPlayers_LeagueId_PlayerId (one club per player per league) cannot be kept without LeagueId on the row; that rule now lives in the squad create and update validators.
-- UQ_LeagueTeamPlayers_LeagueTeamId_SquadNumber replaces the (TeamId, LeagueId, SquadNumber) key and leads on LeagueTeamId, so it also backs the cascade and the GetLeagueTeamPlayers lookup.
DECLARE @Now DATETIME2(7) = SYSUTCDATETIME();
DECLARE @ImportUserId UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000001';

INSERT INTO dbo.LeagueTeams (LeagueId, TeamId, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
SELECT pairs.LeagueId, pairs.TeamId, @Now, @Now, @ImportUserId, @ImportUserId
FROM (SELECT DISTINCT LeagueId, TeamId FROM dbo.LeagueTeamPlayers) AS pairs
WHERE NOT EXISTS (SELECT 1 FROM dbo.LeagueTeams lt WHERE lt.LeagueId = pairs.LeagueId AND lt.TeamId = pairs.TeamId);

ALTER TABLE dbo.LeagueTeamPlayers ADD LeagueTeamId INT NULL;
GO

UPDATE ltp
SET LeagueTeamId = lt.Id
FROM dbo.LeagueTeamPlayers ltp
    INNER JOIN dbo.LeagueTeams lt ON lt.LeagueId = ltp.LeagueId AND lt.TeamId = ltp.TeamId;
GO

ALTER TABLE dbo.LeagueTeamPlayers ALTER COLUMN LeagueTeamId INT NOT NULL;

ALTER TABLE dbo.LeagueTeamPlayers DROP CONSTRAINT FK_LeagueTeamPlayers_Leagues;

ALTER TABLE dbo.LeagueTeamPlayers DROP CONSTRAINT FK_LeagueTeamPlayers_Teams;

ALTER TABLE dbo.LeagueTeamPlayers DROP CONSTRAINT UQ_LeagueTeamPlayers_LeagueId_PlayerId;

ALTER TABLE dbo.LeagueTeamPlayers DROP CONSTRAINT UQ_LeagueTeamPlayers_TeamId_LeagueId_SquadNumber;

ALTER TABLE dbo.LeagueTeamPlayers DROP COLUMN LeagueId, TeamId;

ALTER TABLE dbo.LeagueTeamPlayers ADD CONSTRAINT UQ_LeagueTeamPlayers_LeagueTeamId_SquadNumber UNIQUE (LeagueTeamId, SquadNumber);

ALTER TABLE dbo.LeagueTeamPlayers ADD CONSTRAINT FK_LeagueTeamPlayers_LeagueTeams FOREIGN KEY (LeagueTeamId) REFERENCES dbo.LeagueTeams (Id) ON DELETE CASCADE;
