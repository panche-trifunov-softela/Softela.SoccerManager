CREATE OR ALTER PROCEDURE dbo.GetPlayerPositions
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Ordered by Quality DESC so the player's best position comes back first.
    SELECT Id, PlayerId, PositionId, Quality, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.PlayerPositions
    WHERE PlayerId = @PlayerId
    ORDER BY Quality DESC;
END
