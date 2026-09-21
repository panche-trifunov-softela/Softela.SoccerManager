CREATE OR ALTER PROCEDURE dbo.GetFormationPositionById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, FormationId, PositionId, SlotNumber, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.FormationPositions
    WHERE Id = @Id;
END
