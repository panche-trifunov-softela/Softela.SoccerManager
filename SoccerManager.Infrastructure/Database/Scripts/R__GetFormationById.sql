CREATE OR ALTER PROCEDURE dbo.GetFormationById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Formations
    WHERE Id = @Id;
END
