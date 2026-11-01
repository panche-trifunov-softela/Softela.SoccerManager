CREATE OR ALTER PROCEDURE dbo.GetManagerByUserId
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, UserId, ImageUrl, FirstName, LastName, CreatedAt, ModifiedAt, CreatedBy, ModifiedBy
    FROM dbo.Managers
    WHERE UserId = @UserId;
END
