CREATE OR ALTER PROCEDURE dbo.GetPlayerRatings
    @PlayerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, PlayerId, Rating, IsCurrent, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.PlayerRatings
    WHERE PlayerId = @PlayerId
    ORDER BY CreatedAt DESC, Id DESC;
END
