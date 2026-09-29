CREATE OR ALTER PROCEDURE dbo.GetReferees
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, ImageUrl, Tolerance, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Referees
    ORDER BY Name;
END
