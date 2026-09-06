CREATE OR ALTER PROCEDURE dbo.GetLeagues
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Leagues
    ORDER BY Name;
END
