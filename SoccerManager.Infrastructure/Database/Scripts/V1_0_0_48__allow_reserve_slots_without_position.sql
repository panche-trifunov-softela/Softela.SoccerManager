-- V1_0_0_22 is already applied and is never edited; Evolve rejects a changed checksum.
-- A reserve slot (12 to 19) has no fixed position, since any player can sit on the bench; a starter slot (1 to 11) keeps one.
-- So PositionId becomes nullable, and CK_FormationPositions_PositionMatchesSlot holds the rule both ways.
-- FK_FormationPositions_Positions (ON DELETE CASCADE) and IX_FormationPositions_PositionId stay: the ALTER COLUMN works with both in place.
-- A NULL PositionId never matches that cascade, so deleting a position leaves the reserve slots alone.
-- ADD CONSTRAINT checks existing rows: if a slot 12 to 19 already has a position, it fails and Evolve rolls the script back.
ALTER TABLE dbo.FormationPositions ALTER COLUMN PositionId INT NULL;

ALTER TABLE dbo.FormationPositions ADD CONSTRAINT CK_FormationPositions_PositionMatchesSlot
    CHECK ((SlotNumber <= 11 AND PositionId IS NOT NULL) OR (SlotNumber >= 12 AND PositionId IS NULL));
