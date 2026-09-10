CREATE OR ALTER PROCEDURE dbo.InsertPlayerPosition
    @PlayerId   INT,
    @PositionId INT,
    @Quality    INT,
    @CreatedAt  DATETIME2(7),
    @ModifiedAt DATETIME2(7),
    @CreatedBy  UNIQUEIDENTIFIER,
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.PlayerPositions (PlayerId, PositionId, Quality, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy)
    OUTPUT INSERTED.Id
    VALUES (@PlayerId, @PositionId, @Quality, @CreatedAt, @ModifiedAt, @CreatedBy, @ModifiedBy);
END
