CREATE OR ALTER PROCEDURE dbo.GetManagerById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, UserId, ImageUrl, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Managers
    WHERE Id = @Id;
END
