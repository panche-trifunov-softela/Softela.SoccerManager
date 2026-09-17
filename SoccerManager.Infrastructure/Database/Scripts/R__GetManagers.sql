CREATE OR ALTER PROCEDURE dbo.GetManagers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, UserId, ImageUrl, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Managers
    ORDER BY Id;
END
