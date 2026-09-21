CREATE OR ALTER PROCEDURE dbo.UpdateFormationPosition
    @Id          INT,
    @FormationId INT,
    @PositionId  INT,
    @SlotNumber  INT,
    @ModifiedAt  DATETIME2(7),
    @ModifiedBy  UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.FormationPositions
    SET FormationId = @FormationId,
        PositionId  = @PositionId,
        SlotNumber  = @SlotNumber,
        ModifiedAt  = @ModifiedAt,
        ModifiedBy  = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
