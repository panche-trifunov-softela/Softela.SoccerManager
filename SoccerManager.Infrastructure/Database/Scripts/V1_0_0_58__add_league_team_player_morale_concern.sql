-- V1_0_0_19 is already applied and is never edited; Evolve rejects a changed checksum.
-- MoraleConcern is the reason behind a squad member's morale, when there is one. NULL means there is none, which is every row that exists today, so the column is nullable with no default.
-- MoraleConcern holds a SoccerManager.Domain.Enums.MoraleConcern value (1 PlayingTime, 2 SquadNumber, 3 ContractLength, 4 Wage, 5 TransferListed), and CK_LeagueTeamPlayers_MoraleConcernKnown keeps any other value out, so a new reason needs a new script as well as a new enum member.
-- Nothing writes it yet: the operator POST and PUT do not carry it, and R__UpdateLeagueTeamPlayer names its columns, so an update leaves it as it is.
-- A CHECK passes when its condition is NULL, so every existing row passes it.
-- GO is required: SQL Server compiles a whole batch before running it, so the CHECK cannot name a column the same batch adds (V1_0_0_15). Evolve still runs both batches in one transaction.
ALTER TABLE dbo.LeagueTeamPlayers ADD MoraleConcern TINYINT NULL;
GO

ALTER TABLE dbo.LeagueTeamPlayers ADD CONSTRAINT CK_LeagueTeamPlayers_MoraleConcernKnown CHECK (MoraleConcern BETWEEN 1 AND 5);
