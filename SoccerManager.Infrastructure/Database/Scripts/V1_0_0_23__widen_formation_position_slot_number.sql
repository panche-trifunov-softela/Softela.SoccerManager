-- V1_0_0_22 shipped CK_FormationPositions_SlotNumber as 1-11, covering only the eleven starters. A formation also names its reserves, so the range widens to 1-19 to match the InclusiveBetween(1, 19) rule in the create and update validators.
-- V1_0_0_22 itself is deliberately not edited: it is already merged and may already be applied, and Evolve rejects a versioned script whose checksum changed.
-- Widening a CHECK can never fail on existing rows, since every value the old constraint allowed the new one allows too.
ALTER TABLE dbo.FormationPositions DROP CONSTRAINT CK_FormationPositions_SlotNumber;

ALTER TABLE dbo.FormationPositions ADD CONSTRAINT CK_FormationPositions_SlotNumber CHECK (SlotNumber BETWEEN 1 AND 19);
