CREATE OR ALTER PROCEDURE dbo.UpdatePlayerPosition
    @Id         INT,
    @PlayerId   INT,
    @PositionId INT,
    @Quality    INT,
    @ModifiedAt DATETIME2(7),
    @ModifiedBy UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.PlayerPositions
    SET PlayerId   = @PlayerId,
        PositionId = @PositionId,
        Quality    = @Quality,
        ModifiedAt = @ModifiedAt,
        ModifiedBy = @ModifiedBy
    WHERE Id = @Id;

    SELECT @Id;
END
