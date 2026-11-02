CREATE OR ALTER PROCEDURE dbo.GetFormationPositionById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- PitchX and PitchY are where the slot sits on the pitch (0 to 100, from the team's own side); NULL on a reserve slot, and on a starter slot until it has been placed.
    SELECT Id, FormationId, PositionId, SlotNumber, PitchX, PitchY, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.FormationPositions
    WHERE Id = @Id;
END
