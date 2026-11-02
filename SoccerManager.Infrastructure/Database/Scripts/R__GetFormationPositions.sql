CREATE OR ALTER PROCEDURE dbo.GetFormationPositions
    @FormationId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by SlotNumber so the formation comes back in its own slot order, the eleven starters first and then the reserves.
    -- PitchX and PitchY are where the slot sits on the pitch (0 to 100, from the team's own side); NULL on a reserve slot, and on a starter slot until it has been placed.
    SELECT Id, FormationId, PositionId, SlotNumber, PitchX, PitchY, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.FormationPositions
    WHERE FormationId = @FormationId
    ORDER BY SlotNumber;
END
