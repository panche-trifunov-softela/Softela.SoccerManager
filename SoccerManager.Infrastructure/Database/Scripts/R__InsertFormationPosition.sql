CREATE OR ALTER PROCEDURE dbo.InsertFormationPosition
    @FormationId INT,
    @PositionId  INT,
    @SlotNumber  INT,
    @CreatedAt   DATETIME2(7),
    @ModifiedAt  DATETIME2(7),
    @CreatedBy   UNIQUEIDENTIFIER,
    @ModifiedBy  UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.FormationPositions (FormationId, PositionId, SlotNumber, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@FormationId, @PositionId, @SlotNumber, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
