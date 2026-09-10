CREATE OR ALTER PROCEDURE dbo.GetPlayerPositionById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, PlayerId, PositionId, Quality, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.PlayerPositions
    WHERE Id = @Id;
END
