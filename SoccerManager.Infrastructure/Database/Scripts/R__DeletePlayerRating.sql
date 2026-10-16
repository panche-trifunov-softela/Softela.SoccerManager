CREATE OR ALTER PROCEDURE dbo.DeletePlayerRating
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.PlayerRatings WHERE Id = @Id;
END
