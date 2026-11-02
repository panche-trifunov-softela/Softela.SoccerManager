-- Sets where a formation slot sits on the pitch (PitchX and PitchY, 0 to 100, from the team's own side); POST and PUT
-- /api/formation-positions run it next to dbo.InsertFormationPosition and dbo.UpdateFormationPosition, in the same transaction.
-- It is a procedure of its own, and those two are unchanged, because the database is shared with local development: an older
-- build re-applies its own text of every procedure it knows, which would strip new parameters, but never touches one it does not.
-- Two NULLs clear the position. Only these two columns are written; the insert or update beside it stamped ModifiedAt/ModifiedBy.
CREATE OR ALTER PROCEDURE dbo.UpdateFormationPositionPitch
    @Id     INT,
    @PitchX TINYINT,
    @PitchY TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.FormationPositions
    SET PitchX = @PitchX,
        PitchY = @PitchY
    WHERE Id = @Id;
END
