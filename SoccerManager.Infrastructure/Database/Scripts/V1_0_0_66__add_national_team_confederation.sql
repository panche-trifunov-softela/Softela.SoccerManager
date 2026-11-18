-- V1_0_0_30 is already applied and is never edited; Evolve rejects a changed checksum.
-- Confederation is the football confederation a national team belongs to, and NULL means none is set. No migration fills it: national teams are data loaded outside the migrations, so their confederations are set outside them too.
-- Confederation holds a SoccerManager.Domain.Enums.Confederation value, and CK_NationalTeams_ConfederationKnown keeps any other value out, so a new confederation needs a new script as well as a new enum member.
-- Nothing in this repository writes it: the operator POST and PUT and the importer do not carry it, and R__UpdateNationalTeam names its columns, so an update leaves it as it is.
-- A CHECK passes when its condition is NULL, so every existing row passes it.
-- GO is required: SQL Server compiles a whole batch before running it, so the CHECK cannot name a column the same batch adds (V1_0_0_15). Evolve still runs both batches in one transaction.
ALTER TABLE dbo.NationalTeams ADD Confederation TINYINT NULL;
GO

ALTER TABLE dbo.NationalTeams ADD CONSTRAINT CK_NationalTeams_ConfederationKnown CHECK (Confederation BETWEEN 1 AND 6);
