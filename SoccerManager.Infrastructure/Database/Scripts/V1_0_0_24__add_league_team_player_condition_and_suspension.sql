-- V1_0_0_19 is already applied and is never edited; Evolve rejects a changed checksum.
-- All three columns are NOT NULL with a named default, because ALTER TABLE ... ADD of a NOT NULL column fails on a table that already has rows.
-- Condition defaults to 100: an existing registration has never been worn down, and full condition is also where a new signing starts.
-- CK_LeagueTeamPlayers_Condition mirrors the validators' InclusiveBetween(1, 100), matching the Quality, SquadNumber and ContractLength precedent for a bounded non-enum INT.
-- The two suspension flags default to 0, since no existing registration is serving a ban. Neither gets a CHECK: BIT admits only 0 and 1, as V1_0_0_18 notes for Match.IsStarted.
ALTER TABLE dbo.LeagueTeamPlayers ADD Condition INT NOT NULL CONSTRAINT DF_LeagueTeamPlayers_Condition DEFAULT 100;

ALTER TABLE dbo.LeagueTeamPlayers ADD IsSuspendedDomesticCompetition BIT NOT NULL CONSTRAINT DF_LeagueTeamPlayers_IsSuspendedDomesticCompetition DEFAULT 0;

ALTER TABLE dbo.LeagueTeamPlayers ADD IsSuspendedContinentalCompetition BIT NOT NULL CONSTRAINT DF_LeagueTeamPlayers_IsSuspendedContinentalCompetition DEFAULT 0;
GO

ALTER TABLE dbo.LeagueTeamPlayers ADD CONSTRAINT CK_LeagueTeamPlayers_Condition CHECK (Condition BETWEEN 1 AND 100);
