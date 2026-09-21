CREATE OR ALTER PROCEDURE dbo.GetFormationPositions
    @FormationId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by SlotNumber so the formation comes back in its own slot order, 1 to 11.
    SELECT Id, FormationId, PositionId, SlotNumber, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.FormationPositions
    WHERE FormationId = @FormationId
    ORDER BY SlotNumber;
END
