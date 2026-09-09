CREATE OR ALTER PROCEDURE dbo.GetPositionById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Area, Side, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Positions
    WHERE Id = @Id;
END
