CREATE OR ALTER PROCEDURE dbo.GetFormations
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Formations
    ORDER BY Name;
END
