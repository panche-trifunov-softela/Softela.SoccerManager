CREATE OR ALTER PROCEDURE dbo.GetPositions
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Area, Side, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Positions
    ORDER BY Name;
END
