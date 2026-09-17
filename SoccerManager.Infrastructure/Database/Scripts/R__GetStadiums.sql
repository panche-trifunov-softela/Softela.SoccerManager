CREATE OR ALTER PROCEDURE dbo.GetStadiums
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, ImageUrl, Size, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Stadiums
    ORDER BY Name;
END
