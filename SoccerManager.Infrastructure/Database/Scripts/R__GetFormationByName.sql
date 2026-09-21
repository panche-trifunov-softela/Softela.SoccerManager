-- Backs the uniqueness rule in CreateFormationValidator and UpdateFormationValidator; the database collation is case-insensitive, so this agrees with UQ_Formations_Name on what counts as the same name.
CREATE OR ALTER PROCEDURE dbo.GetFormationByName
    @Name NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Formations
    WHERE Name = @Name;
END
