-- V1_0_0_09 and V1_0_0_11 are already applied and are never edited, since Evolve rejects a changed checksum, so this index is added as its own script rather than folded into either.
-- ManagerId is only the third column of UQ_LeagueTeamManagers_LeagueId_TeamId_ManagerId_StartDate, so that constraint cannot serve a lookup or join keyed on ManagerId alone.
-- This index backs both the new GetLeagueTeamManagersByUserId join and the ON DELETE CASCADE of FK_LeagueTeamManagers_Managers, which V1_0_0_11 added without one; IX_LeagueTeamPlayers_PlayerId is the precedent for indexing a cascade this way.
CREATE NONCLUSTERED INDEX IX_LeagueTeamManagers_ManagerId ON dbo.LeagueTeamManagers (ManagerId);
