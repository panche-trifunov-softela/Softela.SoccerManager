CREATE OR ALTER PROCEDURE dbo.GetStadiumById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, ImageUrl, Size, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Stadiums
    WHERE Id = @Id;
END
