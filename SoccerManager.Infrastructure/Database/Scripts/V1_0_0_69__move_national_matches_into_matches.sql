-- V1_0_0_57 and V1_0_0_67 are already applied and are never edited; Evolve rejects a changed checksum.
-- National matches now live in dbo.Matches: their sides are national team rows on the League's roster (V1_0_0_68) and their competition is the League's national competition, its World Cup, so dbo.NationalTeamMatches (V1_0_0_67) is dropped with its five procedures. The script first stops with an error, changing nothing, if that table has a row; such rows must be moved or deleted by hand before it can run (V1_0_0_40 is the precedent).
-- Stage gains the World Cup stages of SoccerManager.Domain.Enums.MatchStage, 10 (qualifier group) to 16 (final), so CK_Matches_StageKnown is dropped and re-added as BETWEEN 1 AND 16; a CHECK cannot be altered (V1_0_0_54 is the precedent).
-- GroupNumber is the group of a World Cup group match, counted from 1, and NULL on every other match. Like RoundNumber and Leg it only means something on a match with a Stage, so CK_Matches_GroupNeedsStage refuses it on a league match or a friendly.
-- CK_Matches_WorldCupStageHasCompetition keeps a World Cup stage on a match that has a competition; dbo.InsertStagedMatch checks that the competition is the national one.
-- The procedures are dropped with IF EXISTS because Evolve runs every versioned script before any repeatable one, so on a new database this script runs before they exist; their R__ files are deleted in the same change (V1_0_0_28).
-- GO is required: SQL Server compiles a whole batch before running it, so the CHECKs cannot name a column the same batch adds (V1_0_0_15). Evolve still runs all batches in one transaction.
IF EXISTS (SELECT 1 FROM dbo.NationalTeamMatches)
    THROW 50000, N'V1_0_0_69: dbo.NationalTeamMatches still has rows. Move or delete them, then start the API again.', 1;
GO

ALTER TABLE dbo.Matches DROP CONSTRAINT CK_Matches_StageKnown;
ALTER TABLE dbo.Matches ADD GroupNumber TINYINT NULL;
GO

ALTER TABLE dbo.Matches ADD CONSTRAINT CK_Matches_StageKnown CHECK (Stage BETWEEN 1 AND 16);
ALTER TABLE dbo.Matches ADD CONSTRAINT CK_Matches_GroupNumberPositive CHECK (GroupNumber >= 1);
ALTER TABLE dbo.Matches ADD CONSTRAINT CK_Matches_GroupNeedsStage CHECK (GroupNumber IS NULL OR Stage IS NOT NULL);
ALTER TABLE dbo.Matches ADD CONSTRAINT CK_Matches_WorldCupStageHasCompetition CHECK (Stage IS NULL OR Stage <= 9 OR CompetitionId IS NOT NULL);

DROP TABLE dbo.NationalTeamMatches;

DROP PROCEDURE IF EXISTS dbo.InsertNationalTeamMatch;
DROP PROCEDURE IF EXISTS dbo.UpdateNationalTeamMatch;
DROP PROCEDURE IF EXISTS dbo.DeleteNationalTeamMatch;
DROP PROCEDURE IF EXISTS dbo.GetNationalTeamMatchById;
DROP PROCEDURE IF EXISTS dbo.GetNationalTeamMatches;
