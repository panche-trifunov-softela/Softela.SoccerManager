CREATE OR ALTER PROCEDURE dbo.GetPlayerRatingById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, PlayerId, Rating, IsCurrent, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.PlayerRatings
    WHERE Id = @Id;
END
