-- V1_0_0_20 and V1_0_0_25 name some of these procedures in their comments; they are already applied and are never edited, since Evolve rejects a changed checksum.
-- The match team tactic and match formation player position controllers are gone, because any signed-in user could read a rival's lineup and instructions through their GETs, so these seven procedures have no caller left.
-- They are dropped with IF EXISTS, not a bare DROP PROCEDURE, because Evolve runs every versioned (V*) script before any repeatable (R__) script, so on a brand-new database this migration runs at a point where they have never been created; their R__ files are deleted in this same change.

DROP PROCEDURE IF EXISTS dbo.GetMatchTeamTactics;
DROP PROCEDURE IF EXISTS dbo.GetMatchTeamTacticById;
DROP PROCEDURE IF EXISTS dbo.UpdateMatchTeamTactic;
DROP PROCEDURE IF EXISTS dbo.DeleteMatchTeamTactic;
DROP PROCEDURE IF EXISTS dbo.GetMatchFormationPlayerPositions;
DROP PROCEDURE IF EXISTS dbo.GetMatchFormationPlayerPositionById;
DROP PROCEDURE IF EXISTS dbo.UpdateMatchFormationPlayerPosition;
