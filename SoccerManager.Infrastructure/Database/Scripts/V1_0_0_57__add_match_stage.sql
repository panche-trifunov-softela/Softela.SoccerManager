-- V1_0_0_13 is already applied and is never edited; Evolve rejects a changed checksum.
-- Stage, RoundNumber and Leg place a cup or playoff match in its competition. All three are NULL on league matches and friendlies, which is every row that exists today and every row dbo.InsertMatch writes, so they are nullable with no default. The Generator writes them through dbo.InsertStagedMatch.
-- Stage holds a SoccerManager.Domain.Enums.MatchStage value, and CK_Matches_StageKnown keeps any other value out, so a new stage needs a new script as well as a new enum member.
-- RoundNumber is the domestic cup round or the continental league-phase matchday, counted from 1, and Leg is 1 or 2 in a two-legged tie. Both only mean something on a match with a Stage, so CK_Matches_RoundNeedsStage and CK_Matches_LegNeedsStage refuse them on a league match or a friendly.
-- A CHECK passes when its condition is NULL, so each one below judges only a value that is there. Every existing row passes all five, since all of them read NULL.
-- GO is required: SQL Server compiles a whole batch before running it, so the CHECKs cannot name columns the same batch adds (V1_0_0_15). Evolve still runs both batches in one transaction.
ALTER TABLE dbo.Matches ADD Stage TINYINT NULL, RoundNumber TINYINT NULL, Leg TINYINT NULL;
GO

ALTER TABLE dbo.Matches ADD CONSTRAINT CK_Matches_StageKnown CHECK (Stage BETWEEN 1 AND 9);
ALTER TABLE dbo.Matches ADD CONSTRAINT CK_Matches_RoundNumberPositive CHECK (RoundNumber >= 1);
ALTER TABLE dbo.Matches ADD CONSTRAINT CK_Matches_LegFirstOrSecond CHECK (Leg IN (1, 2));
ALTER TABLE dbo.Matches ADD CONSTRAINT CK_Matches_RoundNeedsStage CHECK (RoundNumber IS NULL OR Stage IS NOT NULL);
ALTER TABLE dbo.Matches ADD CONSTRAINT CK_Matches_LegNeedsStage CHECK (Leg IS NULL OR Stage IS NOT NULL);
