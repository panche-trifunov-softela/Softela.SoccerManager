-- V1_0_0_22 is already applied and is never edited; Evolve rejects a changed checksum.
-- PitchX and PitchY place a formation slot on the pitch as whole percentages, seen from the team's own side: X 0 is its left touchline and 100 its right, Y 0 is its own goal line and 100 the opponent's. Which way the UI draws it is the UI's business.
-- TINYINT matches Positions.CatalogOrder, the repo's other small bounded number; CK_FormationPositions_PitchRange holds the 0 to 100 range the validators enforce.
-- Both are nullable: the starter slots that exist when this script runs get their values from a separate data write, and an operator may create a starter slot before placing it.
-- CK_FormationPositions_PitchBothOrNeither mirrors the validators' rule that a slot has both coordinates or neither, and CK_FormationPositions_PitchOnlyOnStarters their rule that a reserve slot (12 to 19), which has no place on the pitch, keeps both NULL. Every existing row holds NULL in both, so no CHECK can fail on existing data.
-- The CHECKs are added in their own batch because they reference columns the first statement adds.
ALTER TABLE dbo.FormationPositions ADD PitchX TINYINT NULL, PitchY TINYINT NULL;
GO

ALTER TABLE dbo.FormationPositions ADD CONSTRAINT CK_FormationPositions_PitchRange
    CHECK ((PitchX IS NULL OR PitchX BETWEEN 0 AND 100) AND (PitchY IS NULL OR PitchY BETWEEN 0 AND 100));

ALTER TABLE dbo.FormationPositions ADD CONSTRAINT CK_FormationPositions_PitchBothOrNeither
    CHECK ((PitchX IS NULL AND PitchY IS NULL) OR (PitchX IS NOT NULL AND PitchY IS NOT NULL));

ALTER TABLE dbo.FormationPositions ADD CONSTRAINT CK_FormationPositions_PitchOnlyOnStarters
    CHECK (SlotNumber <= 11 OR (PitchX IS NULL AND PitchY IS NULL));
